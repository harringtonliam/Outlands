using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Environment;

namespace RPG.WeatherControl
{
    public class CameraWeatherVFX : MonoBehaviour
    {
        [SerializeField] WeatherVFX[] weatherVFXes;
        [SerializeField] WeatherContoller weatherContoller;
        [SerializeField] bool WeatherEffectsOn = true;



        CameraEnvironmentController cameraEnvironmentController;

        [System.Serializable]
        public struct WeatherVFX
        {
            [SerializeField] public Weathers weather;
            [SerializeField]  public GameObject vfx;
        }

        // Start is called before the first frame update
        void Start()
        {
            cameraEnvironmentController = GetComponent<CameraEnvironmentController>();

            if (weatherContoller != null)
            {
                weatherContoller.weatherHasChanged += SetVFX;
            }

            cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors += SetVFX;
        }

        private void OnDisable()
        {
            try
            {
                cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors -= SetVFX;
            }
            catch (System.Exception e)
            {

                Debug.Log(System.String.Format("CamereWeatherVFX {0}" , e.Message));
            }
        }



        public void SetVFX()
        {
            foreach (var weatherVFX in weatherVFXes)
            {
                weatherVFX.vfx.SetActive(false);
            }
            if (!WeatherEffectsOn) return;
            if (IsInteriorLocation()) return;

            foreach (var weatherVFX in weatherVFXes)
            {
                if (weatherContoller.CurrentWeather == weatherVFX.weather)
                {
                    weatherVFX.vfx.SetActive(true);
                }
            }
        }

        private bool IsInteriorLocation()
        {
            return cameraEnvironmentController.IsCameraAtInteriorLocation();
        }
    }

}





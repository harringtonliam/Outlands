using RPG.WeatherControl;
using UnityEngine;
using System;
using RPG.Environment;

namespace RPG.WeatherControl
{

    public class WeatherSFX : MonoBehaviour
    {
        [SerializeField] WeatherSoundFX[] weatherSFXes;
        [SerializeField] WeatherContoller weatherContoller;
        [SerializeField] bool WeatherEffectsOn = true;



        CameraEnvironmentController cameraEnvironmentController;

        [System.Serializable]
        public struct WeatherSoundFX
        {
            [SerializeField] public Weathers weather;
            [SerializeField] public AudioSource sfx;
        }

        // Start is called before the first frame update
        void Start()
        {


            if (weatherContoller != null)
            {
                weatherContoller.weatherHasChanged += SetSFX;
            }

            cameraEnvironmentController = GetComponent<CameraEnvironmentController>();
            cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors += SetSFX;

            SetSFX();
        }

        private void OnDisable()
        {
            try
            {
                cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors -= SetSFX;
            }
            catch (Exception e)
            {

                Debug.Log(System.String.Format("WeatherSFX {0}", e.Message));
            }
        }


        public void SetSFX()
        {
            foreach (var weatherVFX in weatherSFXes)
            {
                weatherVFX.sfx.Stop();
            }
            if (!WeatherEffectsOn) return;
            if (IsInteriorLocation()) return;

            foreach (var weatherVFX in weatherSFXes)
            {
                if (weatherContoller.CurrentWeather == weatherVFX.weather)
                {
                    weatherVFX.sfx.Play();
                }
            }
        }

        private bool IsInteriorLocation()
        {
            return cameraEnvironmentController.IsCameraAtInteriorLocation();
        }
    }
}

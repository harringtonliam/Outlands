using RPG.WeatherControl;
using UnityEngine;

namespace RPG.WeatherControl
{

    public class WeatherSFX : MonoBehaviour
    {
        [SerializeField] WeatherSoundFX[] weatherSFXes;
        [SerializeField] WeatherContoller weatherContoller;
        [SerializeField] bool WeatherEffectsOn = true;
        [SerializeField] float interiorLoactionHeight = -50f;


        private float storedPositionY;

        [System.Serializable]
        public struct WeatherSoundFX
        {
            [SerializeField] public Weathers weather;
            [SerializeField] public AudioSource sfx;
        }

        // Start is called before the first frame update
        void Start()
        {
            storedPositionY = transform.position.y;

            if (weatherContoller != null)
            {
                weatherContoller.weatherHasChanged += SetSFX;
            }
            SetSFX();
        }

        private void Update()
        {
            if (Mathf.Abs(storedPositionY - transform.position.y) > 20)
            {
                storedPositionY = transform.position.y;
                SetSFX();
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
            if (transform.position.y <= interiorLoactionHeight)
            {
                return true;
            }
            return false;
        }
    }
}

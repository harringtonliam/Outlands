using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using RPG.WeatherControl;
using System.IO;

namespace  RPG.GameTime
{
    public class SunIntensityController : MonoBehaviour
    {
        [SerializeField] WeatherContoller weatherContoller;
        [SerializeField] float dayTimeIntensity = 1f;
        [SerializeField] float nightTimeIntensity = 0.1f;
        [SerializeField] float dayTimeEnvironmnetLightingIntensityMultiplier = 1f;
        [SerializeField] float nighTimeEnvironmnetLightingIntensityMultiplier = 0.05f;
        [SerializeField] float duskFraction = 0.5f;


        GameTimeContoller gameTimeController;
        SunDirectionController sunDirectionController;
        Light sun;


        // Start is called before the first frame update
        void Start()
        {
            gameTimeController = GetComponent<GameTimeContoller>();
            sunDirectionController = GetComponent<SunDirectionController>();
            gameTimeController.hourHasPassed += SetSunProperties;
            weatherContoller.weatherHasChanged += ApplyWeatherEffects;
            sun = sunDirectionController.SunDirectionalLight;
        }

        private void OnDisable()
        {
            try
            {
                gameTimeController.hourHasPassed -= SetSunProperties;
                weatherContoller.weatherHasChanged -= ApplyWeatherEffects;

            } catch(Exception e)
            {
                Debug.Log("SunIntensityController " + e.ToString());
            }

        }

        private void SetSunProperties()
        {
            if(sunDirectionController.IsDawnOrDusk())
            {
                SetDawnOrDuskProperties();
            }
            else if (sunDirectionController.IsDayTime())
            {
                SetDayTimeProperties();
            }
            else
            {
                SetNightTimeProperties();
            }


        }

        private void SetDawnOrDuskProperties()
        {
            //Debug.Log("SETTING DAWNDUSK TIME");
            sun.intensity = dayTimeIntensity * duskFraction * GetWeatherLightIntensityAdjustment();
            sun.shadowStrength = GetWeatherShadowStrenght();
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier * duskFraction;
        }

        private void SetNightTimeProperties()
        {
            sun.intensity = nightTimeIntensity * GetWeatherLightIntensityAdjustment();
            sun.shadowStrength = GetWeatherShadowStrenght();
            RenderSettings.ambientIntensity = nighTimeEnvironmnetLightingIntensityMultiplier;
        }

        private void SetDayTimeProperties()
        {
            sun.intensity = dayTimeIntensity * GetWeatherLightIntensityAdjustment();
            sun.shadowStrength = GetWeatherShadowStrenght();
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier;
        }

        private void ApplyWeatherEffects()
        {
            SetSunProperties();
        }

        private  float GetWeatherLightIntensityAdjustment()
        {
            var currentWeatherEffect =  weatherContoller.GetCurrentWeatherEffect();
            return currentWeatherEffect.LightIntesityPercentage / 100f;
        }

        private float GetWeatherShadowStrenght()
        {
            var currentWeatherEffect = weatherContoller.GetCurrentWeatherEffect();
            return currentWeatherEffect.LightShadowStrenght;
        }




    }

}


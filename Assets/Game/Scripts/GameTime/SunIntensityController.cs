using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace  RPG.GameTime
{
    public class SunIntensityController : MonoBehaviour
    {

        [SerializeField] float dayTimeIntensity = 1f;
        [SerializeField] float dayTimeShadowStrenght = 1f;
        [SerializeField] float nightTimeIntensity = 0.1f;
        [SerializeField] float nightTimeShadowStrenght = 0.1f;
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
            sun = sunDirectionController.SunDirectionalLight;
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
            Debug.Log("SETTING DAWNDUSK TIME");
            sun.intensity = dayTimeIntensity * duskFraction;
            //sun.shadowStrength = dayTimeShadowStrenght * duskFraction;
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier * duskFraction;
        }

        private void SetNightTimeProperties()
        {
            Debug.Log("SETTING NIGHT TIME");
            sun.intensity = nightTimeIntensity;
            //sun.shadowStrength = nightTimeShadowStrenght;
            RenderSettings.ambientIntensity = nighTimeEnvironmnetLightingIntensityMultiplier;
        }

        private void SetDayTimeProperties()
        {
            Debug.Log("SETTING DAY TIME");
            sun.intensity = dayTimeIntensity;
            //sun.shadowStrength = dayTimeShadowStrenght;
            RenderSettings.ambientIntensity = dayTimeEnvironmnetLightingIntensityMultiplier;
        }




    }

}


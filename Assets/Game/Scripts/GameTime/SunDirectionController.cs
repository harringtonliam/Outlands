using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace RPG.GameTime
{
    public class SunDirectionController : MonoBehaviour
    {
        [SerializeField] Light sunDirectionalLight;
        [SerializeField] SunDirectionConfig sunDirectionConfig;
        
        [SerializeField] float maxRotation = 359f;

        GameTimeContoller gameTimeContoller;

        // Start is called before the first frame update
        void Start()
        {
            gameTimeContoller = GetComponent<GameTimeContoller>();
            gameTimeContoller.timeUpdate += CalculateSunDirection;
        }

        public bool IsDayTime()
        {
            if (gameTimeContoller == null) return false;
            if (gameTimeContoller.CurrentLocalHour < sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunRiseHour) return false;
            if (gameTimeContoller.CurrentLocalHour > sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunSetHour) return false;
            return true;
        }

        public bool IsDawnOrDusk()
        {
            if (gameTimeContoller == null) return false;
            if (Mathf.Abs(gameTimeContoller.CurrentLocalHour - sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunRiseHour) <= 1) return true;
            if (Mathf.Abs(gameTimeContoller.CurrentLocalHour - sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunSetHour) <= 1) return true;
            return false;
        }

 
        public Light SunDirectionalLight { get {  return sunDirectionalLight; } }


        private void CalculateSunDirection()
        {
            if (sunDirectionalLight == null) return;
            int currentHour = gameTimeContoller.CurrentLocalHour;

            float sunRiseHour = sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunRiseHour;
            float maxHoursFromNoon = 12f - sunRiseHour;
            float currentHoursFromNoon = Mathf.Abs(12f - currentHour);
            float sunRotationMultiplier = (maxHoursFromNoon - currentHoursFromNoon) / maxHoursFromNoon;

            float noonSunAngle = sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).noonSunAngle;
            float newXRotation = (sunRotationMultiplier * noonSunAngle);

            float sunriseAzimuth = sunDirectionConfig.GetDataForMonth(gameTimeContoller.CurrentLocalMonth).sunriseAzimuth;

            float azimuthFraction = (180f - sunriseAzimuth) / maxHoursFromNoon;

            float currentAzimuth = (azimuthFraction * (currentHour - sunRiseHour)) + sunriseAzimuth;

            float newYRotation = ConvertAzimuthToRotation(currentAzimuth);

            Debug.Log("noonSunAngle=" + noonSunAngle + " sunRotationMultiplier=" + sunRotationMultiplier + " timefromNoon=" + Mathf.Abs(12 - currentHour) + " maxHoursFromNoon=" + maxHoursFromNoon + " hourscomptomaxhours=" + (maxHoursFromNoon - Mathf.Abs(12 - currentHour)));

            if (newXRotation >= maxRotation)
            {
                newXRotation = 0f;
            }

            sunDirectionalLight.transform.rotation = Quaternion.Euler(newXRotation, newYRotation, 0f);
        }

        private float ConvertAzimuthToRotation(float currentAzimuth)
        {
            if (currentAzimuth >= 180f) 
            {
                return currentAzimuth - 180f;
            }
            else
            {
                return 360f + currentAzimuth - 180f;
            }
            
        }
    }
}



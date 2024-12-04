using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace RPG.GameTime
{
    public class SunDirectionController : MonoBehaviour
    {
        [SerializeField] Light sunDirectionalLight;
        [SerializeField] SunDirectionConfig sunDirectionConfig;
        
        [SerializeField] float sunRotationOffset = -90f;
        [SerializeField] float maxRotation = 359f;
        [SerializeField] float zRotation = -20f;

        GameTimeContoller gameTimeContoller;

        // Start is called before the first frame update
        void Start()
        {
            gameTimeContoller = GetComponent<GameTimeContoller>();
            gameTimeContoller.timeUpdate += CalculateSunDirection;
        }


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
            Debug.Log("noonSunAngle=" + noonSunAngle + " sunRotationMultiplier=" + sunRotationMultiplier + " timefromNoon=" + Mathf.Abs(12 - currentHour) + " maxHoursFromNoon=" + maxHoursFromNoon + " hourscomptomaxhours=" + (maxHoursFromNoon - Mathf.Abs(12 - currentHour)));

            if (newXRotation >= maxRotation)
            {
                newXRotation = 0f;
            }
            Vector3 sunRotation = new Vector3(newXRotation, 0f, zRotation);
            sunDirectionalLight.transform.eulerAngles = sunRotation;
        }

    }
}



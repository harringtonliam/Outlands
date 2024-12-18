using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.WeatherControl
{
    [System.Serializable]
    public class WeatherEffect 
    {
        [SerializeField] Weathers weather;
        [SerializeField] float lightIntensityPercentage;
        [SerializeField] float lightShadowStrenght;
        [Tooltip("Maxium duration in hours")]
        [SerializeField] int maxDuration = 4;
        public Weathers Weather { get { return weather; } }
        public float LightIntesityPercentage { get { return lightIntensityPercentage; } }
        public float LightShadowStrenght { get { return lightShadowStrenght; } }
        public int MaxDuration { get {  return maxDuration; } }

    }



}



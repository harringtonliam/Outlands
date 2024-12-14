using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.GameTime;

namespace RPG.WeatherControl
{
    [CreateAssetMenu(fileName = "WeatherTable", menuName = "Weather/New WeatherTable", order = 0)]
    public class WeatherTable : ScriptableObject
    {
        [SerializeField] MonthWeather[] monthWeathers;
        [SerializeField] Weathers defaaultWeather;

        public Weathers GetWeather(int month, int diceRoll)
        {
            foreach (var monthWeather in monthWeathers)
            {
                if (monthWeather.Month == month)
                {
                    foreach (var randomWeather in monthWeather.RandomWeathers)
                    {
                        if (randomWeather.IsInRange(diceRoll))
                        {
                            return randomWeather.Weather;
                        }
                    }
                }
            }
            return defaaultWeather;
        }


        [System.Serializable]
        public class RandomWeather
        {
            [SerializeField] Weathers weather;
            [SerializeField] int chanceRangeStart;
            [SerializeField] int chanceRangeEnd;
            public Weathers Weather { get { return weather; } }
            public bool IsInRange(int rangeNumber)
            {
                if (rangeNumber >= chanceRangeStart && rangeNumber <= chanceRangeEnd)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        [System.Serializable]
        public class MonthWeather
        {
            [SerializeField] int month;
            [SerializeField] RandomWeather[] randomWeathers;

            public int Month {  get { return month; } }
            public RandomWeather[] RandomWeathers { get { return randomWeathers; } }
        }



    }

}





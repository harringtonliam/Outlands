using System;
using UnityEngine;
namespace RPG.GameTime
{
    [CreateAssetMenu(fileName = "SunDirectionConfig", menuName = "GameTime/SunDirectionConfig")]
    public class SunDirectionConfig : ScriptableObject
    {
        [SerializeField] LocalTimeConfiguration localTimeConfiguration;
        [SerializeField] SunDirectionData[] sunDirectionDatas;

        [Serializable]
        public struct SunDirectionData
        {
            public int month;
            public int sunRiseHour;
            public int sunSetHour;
            public int noonSunAngle;
            public int sunriseAzimuth;
        }

        public SunDirectionData GetDataForMonth(int month)
        {
            if (month >= sunDirectionDatas.Length || month < 0)
            {
                month = 0;
            }
            return sunDirectionDatas[month];
        }

        public int GetLenghtOfDayForMonth(int month) 
        {
            if (month >= sunDirectionDatas.Length || month < 0)
            {
                month = 0;
            }
            return sunDirectionDatas[month].sunSetHour - sunDirectionDatas[month].sunRiseHour;
        }

        

    }
}



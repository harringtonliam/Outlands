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
        }

    }
}



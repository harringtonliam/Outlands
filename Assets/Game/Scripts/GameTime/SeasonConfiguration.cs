using System;
using UnityEngine;

namespace RPG.GameTime
{
    [CreateAssetMenu(fileName = "SeasonConfiguration", menuName = "GameTime/SeasonConfiguration")]
    public class SeasonConfiguration : ScriptableObject
    {
        [SerializeField] private MonthSeason[] monthSeasons;

        [Serializable]
        public struct MonthSeason
        {
            public Seasons season;
            public string month;
        }

        public Seasons GetSeason(string month)
        {
            foreach (var monthSeason in monthSeasons) 
            {
                if(monthSeason.month == month)
                {
                    return monthSeason.season;
                }
            }

            return Seasons.Summer;

        }

    }


}



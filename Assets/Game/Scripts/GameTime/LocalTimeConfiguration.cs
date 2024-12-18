using UnityEngine;

namespace RPG.GameTime
{
    [CreateAssetMenu(fileName = "LocalTimeConfiguration", menuName = "GameTime/LocalTimeConfiguration")]
    public class LocalTimeConfiguration : ScriptableObject
    {
        [SerializeField] int daysInMonth = 30;
        [SerializeField] int hoursInDay = 23;
        [SerializeField] string[] weekDays;
        [SerializeField] string[] months;
        [SerializeField] int startYear = 163;
        [SerializeField] int startMonth = 1;
        [SerializeField] SeasonConfiguration seasonConfiguration;

        public int HoursInDay { get { return hoursInDay; } }
        public int DaysInMonth {  get { return daysInMonth; } }
        public string[] Months {  get { return months; } }
        public string[] WeekDays { get { return weekDays; } }
        public int StartYear { get {  return startYear; } }
        public int StartMonth { get { return startMonth; } }

        public Seasons GetSeason(string month)
        {
            return seasonConfiguration.GetSeason(month);
        }

    }

}



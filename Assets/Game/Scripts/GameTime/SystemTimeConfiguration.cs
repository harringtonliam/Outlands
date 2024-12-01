using UnityEngine;


namespace RPG.GameTime
{
    [CreateAssetMenu(fileName = "SystemTimeConfiguration", menuName = "GameTime/SystemTimeConfiguration")]
    public class SystemTimeConfiguration : ScriptableObject
    {
        [SerializeField] int hoursInDay = 24;
        [SerializeField] int daysInYear = 365;
        [SerializeField] int startYear = 245;

        public int HoursInDay { get { return hoursInDay; } }
        public int DaysInYear {  get { return daysInYear; } }
        public int StartYear { get { return startYear; } }

    }
}


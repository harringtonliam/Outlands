using UnityEngine;
using RPG.GameTime;

namespace RPG.Buildings
{
    public class HouseSettings : MonoBehaviour
    {
        [SerializeField] GameObject[] dayTimeDestinations;
        [SerializeField] GameObject[] nightTimeDestinations;
        [SerializeField] int nightTimeStartHour =11;
        [SerializeField] int dayTimeStartHour = 7;


        public GameObject[] DayTimeDestinations { get { return dayTimeDestinations; } }
        public GameObject[] NightTimeDestinations { get { return nightTimeDestinations; } }
        public int NightTimeStartHour { get { return nightTimeStartHour; } }
        public int DayTimeStartHour { get { return dayTimeStartHour; } }

        GameTimeContoller gameTimeContoller;


        public bool IsNightTime()
        {
            return gameTimeContoller.CurrentLocalHour >= nightTimeStartHour || gameTimeContoller.CurrentLocalHour < dayTimeStartHour;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameTimeContoller = FindFirstObjectByType<GameTimeContoller>();
        }


    }


}



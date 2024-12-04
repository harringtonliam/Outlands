using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;

namespace RPG.GameTime
{
    public class GameTimeContoller : MonoBehaviour, ISaveable
    {
        [SerializeField] float hourLenghtInRealMinutes = 10f;
        [SerializeField] float timeUpdateIntervalInMinutes = 1f;
        [SerializeField] int startHour = 10;
        [SerializeField] int startDayInMonth = 1;
        [SerializeField] LocalTimeConfiguration localTimeConfiguration;
        [SerializeField] SystemTimeConfiguration systemTimeConfiguration;


        public event Action timeUpdate;
        public event Action hourHasPassed;
        public event Action dayHasPassed;
        public event Action weekHasPassed;
        public event Action monthHasPassed;
        public event Action yearHasPassed;

        //Properties

        float timeSinceStartOfHour = 0f;
        float timeSinceTimeUpdate = 0f;

        private int currentLocalYear;
        private int currentLocalMonth;
        private int currentLocalDayOfMonth;
        private int currentLocalDayOfWeek;
        private int currentLocalHour;
        private int currentSystemHour;
        private int currentSystemDay;
        private int currentSystemYear;

        public int CurrentLocalYear {  get { return currentLocalYear; } }
        public int CurrentLocalMonth{ get { return currentLocalMonth; } }
        public int CurrentLocalDayOfMonth { get { return currentLocalDayOfMonth; } }
        public int CurrentLocalDayOfWeek { get { return currentLocalDayOfWeek; } }
        public int CurrentLocalHour { get { return currentLocalHour; } }
        
        public int CurrentSystemHour {  get { return currentSystemHour; } }
        public int CurrentSystemDay {  get { return currentSystemDay; } }
        public int CurrentSystemYear { get {  return currentSystemYear; } }

 

        // Start is called before the first frame update
        void Start()
        {
            currentLocalHour = startHour;
            currentLocalDayOfWeek = 0;
            currentLocalDayOfMonth = startDayInMonth;
            currentLocalYear = localTimeConfiguration.StartYear;
            currentLocalMonth = GetStartMonth();

            TriggerAllEventActions();
            Debug.Log("GameTimeContoller  start " + CurrentLocalYear + " " + GetCurrentMonth() + " " + CurrentLocalDayOfMonth + " " + GetCurrentDayOfWeek() + " " + CurrentLocalHour);
        }



        // Update is called once per frame
        void Update()
        {
            timeSinceStartOfHour += Time.deltaTime;
            timeSinceTimeUpdate += Time.deltaTime;
            CheckForStartOfHour();
            CheckForTimeUpdate();
        }

        public string GetCurrentDayOfWeek()
        {
            return localTimeConfiguration.WeekDays[currentLocalDayOfWeek];
        }

        public string GetCurrentMonth()
        {
            return localTimeConfiguration.Months[currentLocalMonth];
        }

        public float GetHourExact()
        {
            float exactHour = currentLocalHour;
            exactHour = currentLocalHour + (timeSinceStartOfHour / 60 / hourLenghtInRealMinutes);
            return exactHour;
        }


        private void CheckForTimeUpdate()
        {
            if (((timeSinceTimeUpdate/60) > timeUpdateIntervalInMinutes)  && (timeUpdate!= null))
            {
                timeUpdate();
                timeSinceTimeUpdate = 0f;
            }
        }

        private void CheckForStartOfHour()
        {
            if ((timeSinceStartOfHour / 60) >= hourLenghtInRealMinutes)
            {
                currentLocalHour++;
                currentSystemHour++;
                CheckForNewDay();
                timeSinceStartOfHour = 0f;
                Debug.Log("GameTimeContoller New Hour Started " + CurrentLocalYear + " " + GetCurrentMonth() + " " + CurrentLocalDayOfMonth + " " + GetCurrentDayOfWeek() + " " + CurrentLocalHour) ;
                if (hourHasPassed != null)
                {
                    hourHasPassed();
                }
                CheckForNewSystemTimeDay();

            }
        }

        private void CheckForNewSystemTimeDay()
        {
            if(currentSystemHour >= systemTimeConfiguration.HoursInDay)
            {
                currentSystemHour = 0;
                currentSystemDay++;
                CheckForNewSystemYear();
            }
        }

        private void CheckForNewSystemYear()
        {
            if(currentSystemDay > systemTimeConfiguration.DaysInYear)
            {
                currentSystemDay = 0;
                currentSystemYear++;
            }
        }

        private void CheckForNewDay()
        {
            if (currentLocalHour >= localTimeConfiguration.HoursInDay)
            {
                currentLocalHour = 0;
                currentLocalDayOfWeek++;
                CheckForNewWeek();
                currentLocalDayOfMonth++;
                CheckForNewMonth();
                Debug.Log("GameTimeContoller New DayStarted Started " + currentLocalHour);
                if (dayHasPassed != null)
                {
                    dayHasPassed();
                }

            }
        }

        private void CheckForNewWeek()
        {
            if (currentLocalDayOfWeek > localTimeConfiguration.WeekDays.Length)
            {
                currentLocalDayOfWeek = 0;
                if (weekHasPassed != null)
                {
                    weekHasPassed();
                }

            }
        }

        private void CheckForNewMonth()
        {
            if (currentLocalDayOfMonth > localTimeConfiguration.DaysInMonth)
            {
                currentLocalDayOfMonth = 1;
                currentLocalMonth++;
                CheckForNewYear();
                if (monthHasPassed != null)
                {
                    monthHasPassed();
                }

            }
        }

        private void CheckForNewYear()
        {
            if (currentLocalMonth > localTimeConfiguration.Months.Length)
            {
                currentLocalMonth = 0;
                currentLocalYear++;
                if (yearHasPassed != null)
                {
                    yearHasPassed();
                }
            }
        }

        private void TriggerAllEventActions()
        {
            if (yearHasPassed != null)
            {
                yearHasPassed();
            }
            if (monthHasPassed != null)
            {
                monthHasPassed();
            }
            if (weekHasPassed != null)
            {
                weekHasPassed();
            }
            if (dayHasPassed != null)
            {
                dayHasPassed();
            }
            if (hourHasPassed != null)
            {
                hourHasPassed();
            }
            if (timeUpdate != null)
            {
                timeUpdate();
            }

        }

        private int GetStartMonth()
        {
            return localTimeConfiguration.StartMonth;
        }

        [System.Serializable]
        private struct GameTimeStateData
        {
            public int currentLocalYear;
            public int currentLocalMonth;
            public int currentLocalDayOfMonth;
            public int currentLocalDayOfWeek;
            public int currentLocalHour;
            public int currentSystemYear;
            public int currentSystemDay;
            public int currentSystemHour;
        }

        public object CaptureState()
        {

            GameTimeStateData gameTimeStateData = new GameTimeStateData();
            gameTimeStateData.currentLocalDayOfMonth = currentLocalDayOfMonth;
            gameTimeStateData.currentLocalDayOfWeek = currentLocalDayOfWeek;
            gameTimeStateData.currentLocalHour = currentLocalHour;
            gameTimeStateData.currentLocalMonth = currentLocalMonth;
            gameTimeStateData.currentLocalYear = currentLocalYear;
            gameTimeStateData.currentSystemHour = currentSystemHour;
            gameTimeStateData.currentSystemDay = currentSystemDay;
            gameTimeStateData.currentSystemYear = currentSystemYear;
            return gameTimeStateData;
        }

        public void RestoreState(object state)
        {
            GameTimeStateData gameTimeStateData = (GameTimeStateData)state;
            currentSystemYear = gameTimeStateData.currentSystemYear;
            currentSystemDay = gameTimeStateData.currentSystemDay;
            currentSystemHour = gameTimeStateData.currentSystemHour;
            //TODO calculate local time from system Time
            currentLocalDayOfMonth = gameTimeStateData.currentLocalDayOfMonth ;
            currentLocalDayOfWeek = gameTimeStateData.currentLocalDayOfWeek;
            currentLocalHour = gameTimeStateData.currentLocalHour;
            currentLocalMonth = gameTimeStateData.currentLocalMonth;
            currentLocalYear = gameTimeStateData.currentLocalYear;
            TriggerAllEventActions();
        }
    }


}







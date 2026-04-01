using RPG.Core;
using RPG.EventBus;
using RPG.Events;
using RPG.GameTime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.WeatherControl
{
    public class WeatherContoller : MonoBehaviour
    {

        [SerializeField] WeatherTable weatherTable;
        [SerializeField] WeatherDescriptions weatherDescriptions;


        int currentWeatherDurationHours = 0;
        int currentWeatherHourSoFar = 10;
        Weathers currentWeather;

        public event Action weatherHasChanged;

        public Weathers CurrentWeather {  get { return currentWeather; } }

        void Awake()
        {
            Bus<GameTimeHourHasPassedEvent>.OnEvent += GenerateWeather;
        }



        // Start is called before the first frame update
        void Start()
        {
 
        }

        private void OnDisable()
        {
            try
            {
                Bus<GameTimeHourHasPassedEvent>.OnEvent -= GenerateWeather;
            }
            catch (Exception ex)
            {

                Debug.Log("GameTimeUI " + ex.Message);
            }

        }

        public WeatherEffect GetCurrentWeatherEffect()
        {
            return weatherDescriptions.GetWeatherEffect(currentWeather);
        }

        private void GenerateWeather(GameTimeHourHasPassedEvent evt)
        {
            currentWeatherHourSoFar++;
            if (NewWeatherNeeded())
            {
                int randomWeatherDiceRoll = Dice.RollDice(100, 1);
                Weathers newWeather = weatherTable.GetWeather(evt.GameTimeContoller.GetCurrentMonth(), randomWeatherDiceRoll);
                currentWeatherDurationHours = Mathf.Clamp(Dice.RollDice(4, 1), 1, weatherDescriptions.GetWeatherEffect(newWeather).MaxDuration);
                Debug.Log("Generate Weather " + newWeather + " for hours: " + currentWeatherDurationHours + " dice rolle was " + randomWeatherDiceRoll + " month " + evt.GameTimeContoller.GetCurrentMonth());
                currentWeatherHourSoFar = 0;
                currentWeather = newWeather;
                 if (weatherHasChanged!= null)
                {
                    weatherHasChanged();
                }
            }
        }

        private bool NewWeatherNeeded()
        {
            if (currentWeatherDurationHours <= currentWeatherHourSoFar)
            {
                return true;
            }
            return false;
        }

    }
}



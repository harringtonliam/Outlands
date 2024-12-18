using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.GameTime;
using RPG.Core;
using System;

namespace RPG.WeatherControl
{
    public class WeatherContoller : MonoBehaviour
    {

        [SerializeField] GameTimeContoller gameTimeContoller;
        [SerializeField] WeatherTable weatherTable;
        [SerializeField] WeatherDescriptions weatherDescriptions;


        int currentWeatherDurationHours = 0;
        int currentWeatherHourSoFar = 10;
        Weathers currentWeather;

        float[] lightSourceStartIntensities;

        public event Action weatherHasChanged;

        public Weathers CurrentWeather {  get { return currentWeather; } }

        // Start is called before the first frame update
        void Start()
        {
            gameTimeContoller.hourHasPassed += GenerateWeather;
            GenerateWeather();
        }


        private void GenerateWeather()
        {
            currentWeatherHourSoFar++;
            if (NewWeatherNeeded())
            {
                int randomWeatherDiceRoll = Dice.RollDice(100, 1);
                Weathers newWeather = weatherTable.GetWeather(gameTimeContoller.GetCurrentMonth(), randomWeatherDiceRoll);
                currentWeatherDurationHours = Mathf.Clamp(Dice.RollDice(4, 1), 1, weatherDescriptions.GetWeatherEffect(newWeather).MaxDuration);
                Debug.Log("Generate Weather " + newWeather + " for hours: " + currentWeatherDurationHours + " dice rolle was " + randomWeatherDiceRoll + " month " + gameTimeContoller.GetCurrentMonth());
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



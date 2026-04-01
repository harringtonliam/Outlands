using System;
using UnityEngine;
using RPG.GameTime;
using TMPro;
using System.Text;
using RPG.EventBus;
using RPG.Events;


namespace RPG.UI.GameTime
{
    public class GameTimeUI : MonoBehaviour
    {
        [SerializeField] TMP_Text gameTimetext;


        //private GameTimeContoller gameTimeContoller;

        void Awake()
        {
            Bus<GameTimeHourHasPassedEvent>.OnEvent += DisplayGameTime;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {   
            //gameTimeContoller = FindFirstObjectByType<GameTimeContoller>();
            //gameTimeContoller.hourHasPassed +=  DisplayGameTime;
            //DisplayGameTime();
        }

        private void OnDisable()
        {
              try
            {
                Bus<GameTimeHourHasPassedEvent>.OnEvent -= DisplayGameTime;
            }
            catch (Exception ex)
            {

                Debug.Log("GameTimeUI " + ex.Message);
            }

        }

        private void DisplayGameTime(GameTimeHourHasPassedEvent evt)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(evt.GameTimeContoller.CurrentLocalHour.ToString() + " hours");
            sb.AppendLine(evt.GameTimeContoller.GetCurrentDayOfWeek());
            sb.AppendLine(evt.GameTimeContoller.CurrentLocalDayOfMonth + " " + evt.GameTimeContoller.GetCurrentMonth());
            sb.AppendLine(evt.GameTimeContoller.CurrentLocalYear.ToString());
            gameTimetext.text = sb.ToString();
        }

    }

}



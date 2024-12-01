using System;
using UnityEngine;
using RPG.GameTime;
using TMPro;
using System.Text;


namespace RPG.UI.GameTime
{
    public class GameTimeUI : MonoBehaviour
    {
        [SerializeField] TMP_Text gameTimetext;


        private GameTimeContoller gameTimeContoller;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {   
            gameTimeContoller = FindFirstObjectByType<GameTimeContoller>();
            gameTimeContoller.hourHasPassed +=  DisplayGameTime;
            DisplayGameTime();
        }

        private void OnDisable()
        {
              try
            {
                gameTimeContoller.hourHasPassed -= DisplayGameTime;
            }
            catch (Exception ex)
            {

                Debug.Log("GameTimeUI " + ex.Message);
            }

        }

        private void DisplayGameTime()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Hour: " + gameTimeContoller.CurrentLocalHour);
            sb.AppendLine("Day: "  + gameTimeContoller.CurrentLocalDayOfMonth);
            sb.AppendLine("Month: " + gameTimeContoller.CurrentLocalMonth);
            sb.AppendLine("Year: " + gameTimeContoller.CurrentLocalYear);
            gameTimetext.text = sb.ToString();
        }

    }

}



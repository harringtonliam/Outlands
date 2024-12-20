using UnityEngine;
using RPG.GameTime;
using System;

namespace RPG.Environment
{
    public class SeasonalPlant : MonoBehaviour
    {
        [SerializeField] SeasonalPrefabs[] seasonalPrefabs;

        GameTimeContoller gameTimeContoller;

        [Serializable]
        public struct SeasonalPrefabs
        {
            public Seasons season;
            public GameObject prefab;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gameTimeContoller = FindFirstObjectByType<GameTimeContoller>();
            gameTimeContoller.monthHasPassed += UpdatePlant;
            UpdatePlant();
        }

        private void OnDisable()
        {
            try
            {
                gameTimeContoller.monthHasPassed -= UpdatePlant;

            }
            catch (System.Exception ex)
            {
                Debug.Log("SeasonPlant" + ex.Message);
            }
        }



        private void UpdatePlant()
        {
            foreach (var seasonalPrefab in seasonalPrefabs)
            {
                if(seasonalPrefab.season == gameTimeContoller.GetCurrentSeason())
                {
                    seasonalPrefab.prefab.SetActive(true);
                }
                else
                {
                    seasonalPrefab.prefab.SetActive(false);
                }
            }
        }
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.NightVision
{
    public class InfraredEffect : MonoBehaviour
    {
        [SerializeField] GameObject infraRedEffect;


        NightVisionSettings inGameSettings;



        private void Awake()
        {
            inGameSettings = FindFirstObjectByType<NightVisionSettings>();
        }

        private void OnEnable()
        {
            inGameSettings.SettingsUpdated += SwitchEffect;
        }


        private void OnDisable()
        {
            inGameSettings.SettingsUpdated -= SwitchEffect;
        }


        private  void SwitchEffect()
        {
            if (infraRedEffect == null) return;

            infraRedEffect.SetActive(inGameSettings.IsNightVisionOnAndEquiped());
        }
    }
}



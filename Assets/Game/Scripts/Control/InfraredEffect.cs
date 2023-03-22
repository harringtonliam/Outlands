using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using System;
using RPG.Core;

namespace RPG.Control
{
    public class InfraredEffect : MonoBehaviour
    {
        [SerializeField] GameObject infraRedEffect;

        Equipment playerEquipment;
        NightVisionSettings inGameSettings;



        private void Awake()
        {
            inGameSettings = FindObjectOfType<NightVisionSettings>();
        }


        private void OnDisable()
        {
            inGameSettings.SettingsUpdated -= SwitchEffect;
        }


        private  void SwitchEffect()
        {
            if (infraRedEffect == null) return;

            infraRedEffect.SetActive(inGameSettings.IsNightVisionOn);
        }
    }
}



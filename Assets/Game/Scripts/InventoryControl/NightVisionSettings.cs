using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.InventoryControl
{
    public class NightVisionSettings: MonoBehaviour
    {

        bool isNightVisionOn = false;
        bool isNightVisionEquiped;

        Equipment[] playerEquipments;

        public event Action SettingsUpdated;

        public bool IsNightVisionOn
        {
            get { return isNightVisionOn; }
        }

        public bool IsNightVisionOnAndEquiped()
        {
            return isNightVisionOn & isNightVisionEquiped;
        }

        private void OnEnable()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            playerEquipments = new Equipment[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerEquipments[i] = players[i].GetComponent<Equipment>();
                playerEquipments[i].equipmentUpdated += CheckForNightVisionEquipment;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < playerEquipments.Length; i++)
            {
                playerEquipments[i].equipmentUpdated -= CheckForNightVisionEquipment;
            }
        }


        public void SetIsNighttVisionOn(bool isNightVisionOn)
        {
            this.isNightVisionOn = isNightVisionOn;
            if (SettingsUpdated != null)
            {
                SettingsUpdated();
            }
        }

        private void CheckForNightVisionEquipment()
        {
            isNightVisionEquiped = false;
            for (int i = 0; i < playerEquipments.Length; i++)
            {
                if (playerEquipments[i].GetItemInSlot(EquipLocation.Helmet).IsNightVisionEnabled)
                {
                    isNightVisionEquiped = true;
                }
            }

            if (SettingsUpdated  != null)
            {
                SettingsUpdated();
            }
                
        }



    }
}



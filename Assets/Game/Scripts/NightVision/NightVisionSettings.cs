using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.InventoryControl;
using RPG.Control;
using RPG.Attributes;

namespace RPG.NightVision
{
    public class NightVisionSettings: MonoBehaviour
    {

        bool isNightVisionOn = false;
        bool isNightVisionEquiped;

        Equipment[] playerEquipments;
        PlayerSelector[] playerSelectors;
        Health[] playerHealths;

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
            playerSelectors =  new PlayerSelector[players.Length];
            playerHealths = new Health[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerEquipments[i] = players[i].GetComponent<Equipment>();
                playerEquipments[i].equipmentUpdated += CheckForNightVisionEquipment;
                playerSelectors[i] = players[i].GetComponent<PlayerSelector>();
                playerSelectors[i].selectedUpdated += CheckForNightVisionEquipment;
                playerHealths[i] = players[i].GetComponent<Health>();
                playerHealths[i].deathUpdated += CheckForNightVisionEquipment;
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < playerEquipments.Length; i++)
            {
                playerEquipments[i].equipmentUpdated -= CheckForNightVisionEquipment;
                playerSelectors[i].selectedUpdated -= CheckForNightVisionEquipment;
                playerHealths[i].deathUpdated -= CheckForNightVisionEquipment;
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
                var equipedItem = playerEquipments[i].GetItemInSlot(EquipLocation.Helmet);
                var health = playerEquipments[i].GetComponent<Health>();
                var playerSelctor = playerEquipments[i].GetComponent<PlayerSelector>();
                if (equipedItem != null && equipedItem.IsNightVisionEnabled  && !health.IsDead && playerSelctor.IsSelected)
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



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using RPG.Combat;
using System;

namespace RPG.Stats
{
    public class Encumberance : MonoBehaviour
    {
        CharacterAbilities characterAbilities;
        Inventory inventory;
        Equipment equipment;
        AmmunitionStore ammunitionStore;
        WeaponStore weaponStore;
        QuickItemStore quickItemStore;

        float totalMass = 0f;

        public event Action encumberanceUpdated;

        private void Start()
        {
            characterAbilities = GetComponent<CharacterAbilities>();
            
        }

        private void OnEnable()
        {
            inventory = GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.inventoryUpdated += RecalculateTotalMass;
            }

            equipment = GetComponent<Equipment>();
            if (equipment != null)
            {
                equipment.equipmentUpdated += RecalculateTotalMass;
            }

            ammunitionStore = GetComponent<AmmunitionStore>();
            if (ammunitionStore != null)
            {
                ammunitionStore.storeUpdated += RecalculateTotalMass;
            }

            weaponStore = GetComponent<WeaponStore>();
            if (weaponStore != null)
            {
                weaponStore.storeUpdated += RecalculateTotalMass;
            }

            quickItemStore = GetComponent<QuickItemStore>();
            if (quickItemStore != null)
            {
                quickItemStore.storeUpdated += RecalculateTotalMass;
            }

            RecalculateTotalMass();

        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.inventoryUpdated -= RecalculateTotalMass;
            }
            if (equipment != null)
            {
                equipment.equipmentUpdated -= RecalculateTotalMass;
            }
            if (ammunitionStore != null)
            {
                ammunitionStore.storeUpdated -= RecalculateTotalMass;
            }
            if (weaponStore != null)
            {
                weaponStore.storeUpdated -= RecalculateTotalMass;
            }
            if (quickItemStore != null)
            {
                quickItemStore.storeUpdated -= RecalculateTotalMass;
            }
        }

        public void RecalculateTotalMass()
        {
            totalMass = 0f;
            if (inventory != null)
            {
                totalMass += inventory.GetTotalMass();
            }
            if (equipment != null)
            {
                totalMass += equipment.GetTotalMass();
            }
            if (ammunitionStore != null)
            {
                totalMass += ammunitionStore.GetTotalMass();
            }
            if (weaponStore != null)
            {
                totalMass += weaponStore.GetTotalMass();
            }
            if (quickItemStore != null)
            {
                totalMass += quickItemStore.GetTotalMass();
            }

            if (encumberanceUpdated != null)
            {
                encumberanceUpdated();
            }
        }

        public bool IsEncumbered()
        {
            if(GetMaxUnencumberedCapacity() < totalMass)
            {
                return true;
            }

            return false;
        }

        public float GetMaxUnencumberedCapacity()
        {
            float strenght = characterAbilities.GetAbilityScore(Ability.Strength);
            return strenght / 2;
        }

        public float GetTotalCarried()
        {
            return totalMass;
        }
    }

}



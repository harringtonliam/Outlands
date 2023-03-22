using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InventoryControl
{
    [CreateAssetMenu(menuName = ("Items/Equipable Item"))]
    public class EquipableItem : InventoryItem
    {
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;
        [SerializeField] bool isNightVisionEnabled = false;

        public EquipLocation AllowedEquiplocation
        {
            get { return allowedEquipLocation;  }
        }

        public bool IsNightVisionEnabled { get { return isNightVisionEnabled; } }

    }
}


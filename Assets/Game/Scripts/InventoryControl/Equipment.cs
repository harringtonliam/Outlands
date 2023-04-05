using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.InventoryControl
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        [SerializeField]
        EquipmentSlot[] equippedItems;

        [Serializable]
        public struct EquipmentSlot
        {
            public EquipLocation equipLocation;
            public EquipableItem equipableItem;
            public int number;
            public int remainingUses;
        }


        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            int equipedLocationIndex = IndexOfEquipedLocation(equipLocation);

            if (equipedLocationIndex < 0)
            {
                return null;
            }

            return equippedItems[equipedLocationIndex].equipableItem;
        }

        private int IndexOfEquipedLocation(EquipLocation equipLocation)
        {
            for (int i = 0; i < equippedItems.Length; i++)
            {
                if(equipLocation == equippedItems[i].equipLocation)
                {
                    return i;
                }
            }

            return -1;
        }

        public EquipmentSlot[] GetEquippedItems()
        {
            return equippedItems;
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            AddItem(slot, item, 1, 1);
        }

        public void AddItem(EquipLocation slot, EquipableItem item, int numberOfItems, int remainingUses)
        {
            Debug.Assert(item.AllowedEquiplocation == slot);

            int equipedLocationIndex = IndexOfEquipedLocation(slot);

            equippedItems[equipedLocationIndex].equipableItem = item;
            equippedItems[equipedLocationIndex].number = numberOfItems;
            equippedItems[equipedLocationIndex].remainingUses = remainingUses;

            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }

        }

        public void RemoveItem(EquipLocation equipLocation)
        {
            int equipedLocationIndex = IndexOfEquipedLocation(equipLocation);

            equippedItems[equipedLocationIndex].equipableItem = null;
            equippedItems[equipedLocationIndex].number = 0;
            equippedItems[equipedLocationIndex].remainingUses = 0;
            if (equipmentUpdated != null)
            {
                equipmentUpdated();
            }
        }

        public void UpdateRemainingUses(EquipLocation equipLocation, int changeInUses)
        {
            int equipedLocationIndex = IndexOfEquipedLocation(equipLocation);
            if(equippedItems[equipedLocationIndex].equipableItem.ItemHasUses)
            {
                equippedItems[equipedLocationIndex].remainingUses += changeInUses;
                if (equippedItems[equipedLocationIndex].remainingUses <= 0 && equippedItems[equipedLocationIndex].equipableItem.ItemDestroyedWhenCompletelyUsed)
                {
                    RemoveItem(equipLocation);
                }
            }
        }

        public IEnumerable<EquipLocation> GetAllPopulatedSlots()
        {
            Dictionary<EquipLocation, EquipableItem> equipedSlots = new Dictionary<EquipLocation, EquipableItem>(); 

            for (int i = 0; i < equippedItems.Length; i++)
            {
                if (equippedItems[i].equipableItem != null)
                {
                    equipedSlots.Add(equippedItems[i].equipLocation, equippedItems[i].equipableItem);
                }
            }

            return equipedSlots.Keys;
        }

        [Serializable]
        public struct EquipmentSlotForSerialization 
        {
            public EquipLocation equipLocation;
            public string itemID;
            public int number;
            public int remainingUses;
        }

        public object CaptureState()
        {
            var equippedItemsForSerialization = new EquipmentSlotForSerialization[equippedItems.Length];
            for (int i = 0; i < equippedItems.Length; i++)
            {
                equippedItemsForSerialization[i].equipLocation = equippedItems[i].equipLocation;
                equippedItemsForSerialization[i].itemID = equippedItems[i].equipableItem.ItemID;
                equippedItemsForSerialization[i].number = equippedItems[i].number;
                equippedItemsForSerialization[i].remainingUses = equippedItems[i].remainingUses;
            }

            return equippedItemsForSerialization;
        }

        public void RestoreState(object state)
        {
            var equippedItemsForSerialization = (EquipmentSlotForSerialization[])state;

            for(int i = 0; i< equippedItemsForSerialization.Length; i++)
            {
                var item = (EquipableItem)InventoryItem.GetFromID(equippedItemsForSerialization[i].itemID);
                if (item != null)
                {
                    int equipedLocationIndex = IndexOfEquipedLocation(equippedItemsForSerialization[i].equipLocation);
                    if (equipedLocationIndex > 0)
                    {
                        equippedItems[equipedLocationIndex].equipLocation = equippedItemsForSerialization[i].equipLocation;
                        equippedItems[equipedLocationIndex].equipableItem = item;
                        equippedItems[equipedLocationIndex].number = equippedItemsForSerialization[i].number;
                        equippedItems[equipedLocationIndex].remainingUses = equippedItemsForSerialization[i].remainingUses;
                    }

                }
            }
        }

    }
}




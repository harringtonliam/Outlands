using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Control;
using System;

namespace RPG.InventoryControl
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Maximum Inventory Size")]
        [SerializeField] int inventorySize = 12;

        [SerializeField]
        InventorySlot[] inventorySlots;


        [Serializable]
        public struct InventorySlot
        {
            public InventoryItem inventoryItem;
            public int number;
            public int remainingUses;
        }

        public event Action inventoryUpdated;

        private void Awake()
        {
            InventorySlot[] tempInventorySlots = new InventorySlot[inventorySize];

            if (inventorySlots != null)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    tempInventorySlots[i] = inventorySlots[i];
                }
            }
            inventorySlots = tempInventorySlots;
        }

        public static Inventory GetPlayerInventory()
        {
            var player = PlayerSelector.GetFirstSelectedPlayer();
            return player.GetComponent<Inventory>();
        }

        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }


        public int GetSize()
        {
            return inventorySlots.Length;
        }

        public bool AddToFirstEmptySlot(InventoryItem item, int number, int numberOfUses)
        {
            int i = FindEmptySlot();

            if (i < 0)
            {
                return false;
            }

            inventorySlots[i].inventoryItem = item;
            inventorySlots[i].number += number;
            inventorySlots[i].remainingUses = numberOfUses;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].inventoryItem, item))
                {
                    return true;
                }
            }
            return false;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return inventorySlots[slot].inventoryItem;
        }

        public int GetNumberInSlot(int slot)
        {
            return inventorySlots[slot].number;
        }

        public int GetNumberOfUsesInSlot(int slot)
        {
            return inventorySlots[slot].remainingUses;
        }

        public void RemoveFromSlot(int slot, int number)
        {
            inventorySlots[slot].number -= number;
            if (inventorySlots[slot].number <= 0)
            {
                inventorySlots[slot].number = 0;
                inventorySlots[slot].inventoryItem = null;
                inventorySlots[slot].remainingUses = 0;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }


        public bool AddItemToSlot(int slot, InventoryItem item, int number, int numberOfUses)
        {
            if (inventorySlots[slot].inventoryItem != null)
            {
                return AddToFirstEmptySlot(item, number, numberOfUses); 
            }

            var i = FindStack(item);

            if (i>=0)
            {
                if (inventorySlots[i].number + number > item.MaxNumberInStack)
                {
                    i = -1;
                }
            }

            if (i >= 0)
            {
                slot = i;
            }

            inventorySlots[slot].inventoryItem = item;
            inventorySlots[slot].number += number;
            inventorySlots[slot].remainingUses = numberOfUses;
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
            return true;
        }



        private int FindSlot(InventoryItem item )
        {
            int i = FindStack(item);
            if (i >= 0 && inventorySlots[i].number >= item.MaxNumberInStack)
            {
                i = -1;
            }
            if (i < 0)
            {
                i = FindEmptySlot();
            }
            return i;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].inventoryItem == null)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindStack(InventoryItem item)
        {
            if (!item.IsStackable)
            {
                return -1;
            }

            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (object.ReferenceEquals(inventorySlots[i].inventoryItem, item))
                {
                    return i;
                }
            }
            return -1;
        }

        public float GetTotalMass()
        {
            float inventoryMass = 0f;
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].inventoryItem != null)
                {
                    inventoryMass += inventorySlots[i].inventoryItem.Mass;
                }
            }

            return inventoryMass;
        }

        [System.Serializable]
        private struct InventorySlotRecord
        {
            public string itemID;
            public int number;
            public int remainingUses;
        }

        public object CaptureState()
        {
            var slotStrings = new InventorySlotRecord[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                if (inventorySlots[i].inventoryItem != null)
                {
                    slotStrings[i].itemID = inventorySlots[i].inventoryItem.ItemID;
                    slotStrings[i].number = inventorySlots[i].number;
                    slotStrings[i].remainingUses = inventorySlots[i].remainingUses;
                }
            }
            return slotStrings;
        }

        public void RestoreState(object state)
        {
            var slotStrings = (InventorySlotRecord[])state;
            for (int i = 0; i < inventorySize; i++)
            {
                inventorySlots[i].inventoryItem = InventoryItem.GetFromID(slotStrings[i].itemID);
                inventorySlots[i].number = slotStrings[i].number;
                inventorySlots[i].remainingUses = slotStrings[i].remainingUses;
            }
            if (inventoryUpdated != null)
            {
                inventoryUpdated();
            }
        }


    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;


namespace RPG.InventoryControl
{
    public class QuickItemStore : MonoBehaviour, ISaveable
    {
        [SerializeField]
        DockedItemSlot[] dockedItems = new DockedItemSlot[3];

        [Serializable]
        private class DockedItemSlot
        {
            public ActionItem actionItem;
            public int number;
            public int remainingUses;
        }

        public event Action storeUpdated;


        public ActionItem GetAction(int index)
        {
            if (dockedItems[index] != null)
            {
                return dockedItems[index].actionItem;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            if (dockedItems.Length > index && dockedItems[index] != null)
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        public int GetNumberOfUses(int index)
        {
            if (dockedItems.Length > index && dockedItems[index] != null)
            {
                return dockedItems[index].remainingUses;
            }
            return 0;
        }

        public float GetTotalMass()
        {
            float itemMass = 0f;
            for (int i = 0; i < dockedItems.Length; i++)
            {
                if (dockedItems[i].actionItem != null)
                {
                    itemMass += dockedItems[i].actionItem.Mass;
                }
            }

            return itemMass;
        }

        public void AddAction(InventoryItem item, int index, int number, int numberOfUses)
        {
            if (object.ReferenceEquals(item, dockedItems[index].actionItem))
            {
                dockedItems[index].number += number;
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.actionItem = item as ActionItem;
                slot.number = number;
                slot.remainingUses = numberOfUses;
                dockedItems[index] = slot;
            }
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public bool Use(int index, GameObject user)
        {
            if (dockedItems[(index)] != null)
            {
                dockedItems[index].actionItem.Use(user);
                if (dockedItems[index].actionItem.IsConsumable)
                {
                    RemoveItems(index, 1);
                }
                return true;
            }
            return false;
        }

        public void RemoveItems(int index, int number)
        {
            if (dockedItems[index] != null)
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems[index].actionItem = null;
                    dockedItems[index].number = 0;
                    dockedItems[index].remainingUses = 0;
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }
        }


        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if (!actionItem) return 0;

            if (dockedItems.Length <= index  && !object.ReferenceEquals(item, dockedItems[index].actionItem))
            {
                return 0;
            }
            if (actionItem.IsConsumable)
            {
                return item.MaxNumberInStack;
            }
            if (dockedItems[index].actionItem != null)
            {
                return 0;
            }

            return 1;
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
            public int remainingUses;
        }

        public object CaptureState()
        {
            var state = new  DockedItemRecord[dockedItems.Length];
            for (int i = 0; i < dockedItems.Length; i++)
            {
                if (dockedItems[i].actionItem != null)
                {
                    state[i].itemID = dockedItems[i].actionItem.ItemID;
                    state[i].number = dockedItems[i].number;
                    state[i].remainingUses = dockedItems[i].remainingUses;
                }
            }
            return state;
        }

        public void RestoreState(object state)
        {
            var stateDict = (DockedItemRecord[])state;
            for (int i = 0; i < stateDict.Length; i++)
            {
                AddAction(ActionItem.GetFromID(stateDict[i].itemID) as ActionItem, i, stateDict[i].number, stateDict[i].remainingUses);
            }

            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }
    }
}



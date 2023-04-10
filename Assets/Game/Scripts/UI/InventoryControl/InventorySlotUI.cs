using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Dragging;
using RPG.InventoryControl;

namespace RPG.UI.InventoryControl
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        //Configuration
        [SerializeField] InventoryItemIcon icon = null;

        //State
        int index;
        InventoryItem item;
        Inventory inventory;



        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            InventoryItem itemInSlot = inventory.GetItemInSlot(index);
            int numberToDisplay = 0;
            if (itemInSlot != null && itemInSlot.ItemHasUses)
            {
                numberToDisplay = inventory.GetNumberOfUsesInSlot(index);
            }
            else
            {
                numberToDisplay = inventory.GetNumberInSlot(index);
            }
            icon.SetItem(itemInSlot, numberToDisplay);
        }

        public void AddItems(InventoryItem item, int number, int numberOfUses)
        {
            inventory.AddItemToSlot(index, item, number, numberOfUses);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public int GetNumberOfUses()
        {
            return inventory.GetNumberOfUsesInSlot(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return item.MaxNumberInStack;
            }
            return 0;
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }




    }

}



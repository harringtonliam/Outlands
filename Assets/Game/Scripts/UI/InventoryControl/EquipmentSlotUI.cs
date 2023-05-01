using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using RPG.UI.Dragging;
using RPG.Control;

namespace RPG.UI.InventoryControl
{
    public class EquipmentSlotUI : SelectedPlayerBasedUI, IItemHolder, IDragContainer<InventoryItem>
    {
        //Configuration
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] EquipLocation equipLocation = EquipLocation.Weapon;

        Equipment playerEquipment;

        private void OnEnable()
        {
            var player = PlayerSelector.GetFirstSelectedPlayer();
            playerEquipment = player.GetComponent<Equipment>();
            playerEquipment.equipmentUpdated += RedrawUI;
            RedrawUI();
        }

        private void OnDisable()
        {
            if (playerEquipment == null) return;
            playerEquipment.equipmentUpdated -= RedrawUI;
        }

        public bool AddItems(InventoryItem item, int number, int numberOfUses)
        {
            playerEquipment.AddItem(equipLocation, (EquipableItem)item, number, numberOfUses);
            return true;
        }

        public InventoryItem GetItem()
        {
            return playerEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int GetNumberOfUses()
        {
             return playerEquipment.GetNumberOfUsesinSlot(equipLocation);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            EquipableItem equipableItem = item as EquipableItem;
            if (equipableItem == null) return 0;
            if (equipableItem.AllowedEquiplocation != equipLocation) return 0;
            if (GetItem() != null) return 0;

            return 1;
        }

        public void RemoveItems(int number)
        {
            playerEquipment.RemoveItem(equipLocation);
        }

        public override void SelectedPlayerChanged()
        {
            OnDisable();
            OnEnable();
        }


        void RedrawUI()
        {
            var itemInSlot = playerEquipment.GetItemInSlot(equipLocation);
            int numberToDisplay = 0;
            if (itemInSlot != null && itemInSlot.ItemHasUses)
            {
                numberToDisplay = playerEquipment.GetNumberOfUsesinSlot(equipLocation);
            }
            icon.SetItem(itemInSlot, numberToDisplay);
        }


    }
}



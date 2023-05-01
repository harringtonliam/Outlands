using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using RPG.UI.Dragging;
using RPG.UI.InGame;
using RPG.Control;
using RPG.Core;

namespace RPG.UI.InventoryControl
{

    public class OtherPlayerCharacterSlotUI : MonoBehaviour,  IItemHolder, IDragContainer<InventoryItem>
    {

        public bool AddItems(InventoryItem item, int number, int numberOfUses)
        {
            GameObject playerCharacter = GetComponent<PlayerCharacterUI>().PlayerCharacterGameObject;
            Inventory playerCharacterInventory = playerCharacter.GetComponent<Inventory>();
            if (playerCharacterInventory == null) return false;

            if(!IsCloseEnoughToGiveItem(playerCharacter)) return false;

            return playerCharacterInventory.AddToFirstEmptySlot(item, number, numberOfUses);

        }

        private bool IsCloseEnoughToGiveItem(GameObject playerCharacter)
        {
            GameObject inventorySource =  PlayerSelector.GetFirstSelectedPlayer();
            float distance = Vector3.Distance(playerCharacter.transform.position, inventorySource.transform.position);
            return (distance < ParameterControl.GetFloatValue("MaxDistanceToGiveItem"));
        }

        public InventoryItem GetItem()
        {
            return null;
        }

        public int GetNumber()
        {
            return 0;
        }

        public int GetNumberOfUses()
        {
            return 0;
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return int.MaxValue;
        }

        public void RemoveItems(int number)
        {

        }
    }

}

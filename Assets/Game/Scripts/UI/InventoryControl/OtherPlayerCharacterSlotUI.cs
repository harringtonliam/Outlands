using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using RPG.UI.Dragging;
using RPG.UI.InGame;

namespace RPG.UI.InventoryControl
{

    public class OtherPlayerCharacterSlotUI : MonoBehaviour,  IItemHolder, IDragContainer<InventoryItem>
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void AddItems(InventoryItem item, int number, int numberOfUses)
        {
            GameObject playerCharacter = GetComponent<PlayerCharacterUI>().PlayerCharacterGameObject;
            Inventory playerCharacterInventory = playerCharacter.GetComponent<Inventory>();
            if (playerCharacterInventory == null) return;

            playerCharacterInventory.AddToFirstEmptySlot(item, number, numberOfUses);

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

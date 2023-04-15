using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.InventoryControl;
using RPG.Attributes;
using RPG.Control;


namespace RPG.UI.InventoryControl
{

    public class InventoryTitleUI : SelectedPlayerBasedUI
    {
        [SerializeField] TextMeshProUGUI characterNameText = null;

        Inventory playerInventory;
        CharacterSheet playerCharacterSheet;
        PlayerSelector[] playerSelectors;


        private void OnEnable()
        {
            Redraw();
        }

        private void OnDisable()
        {

        }

        private void Redraw()
        {
            playerInventory = Inventory.GetPlayerInventory();
            playerCharacterSheet = playerInventory.GetComponent<CharacterSheet>();
            if (characterNameText == null) return;
            characterNameText.text = playerCharacterSheet.CharacterName;
            if (playerCharacterSheet.Rank != "")
            {
                characterNameText.text += "  Rank: " + playerCharacterSheet.Rank;
            }


        }

        public override void SelectedPlayerChanged()
        {
            Redraw();
        }
    }

}

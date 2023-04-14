using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RPG.InventoryControl;
using RPG.Attributes;


namespace RPG.UI.InventoryControl
{

    public class InventoryTitleUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI characterNameText = null;

        Inventory playerInventory;
        CharacterSheet playerCharacterSheet;



        private void OnEnable()
        {
            playerInventory = Inventory.GetPlayerInventory();
            playerCharacterSheet = playerInventory.GetComponent<CharacterSheet>();

            Redraw();
        }

        private void OnDisable()
        {
            
        }

        private void Redraw()
        {
            if (characterNameText == null) return;
            characterNameText.text = playerCharacterSheet.CharacterName;
            if (playerCharacterSheet.Rank != "")
            {
                characterNameText.text += "  Rank: " + playerCharacterSheet.Rank;
            }


        }

    }

}

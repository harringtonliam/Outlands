using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;
using UnityEngine.UI;
using TMPro;
using RPG.Attributes;

namespace RPG.UI.InventoryControl
{
    public class OpenContainerUI : MonoBehaviour
    {
        [SerializeField] GameObject uiCanvas = null;
        [SerializeField] InventoryUI containerInventoryUI = null;
        [SerializeField] ScrollRect containerscrollRect;
        [SerializeField] ScrollRect playerscrollRect;
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] TextMeshProUGUI containerName;
        [SerializeField] Image playerImage;
        [SerializeField] Image containerImage;
        [SerializeField] string defaultContainerName;
        [SerializeField] Sprite defaultContainerImage;

        private Inventory containerInventory;
        private Inventory playerInventory;

        void Start()
        {
            uiCanvas.SetActive(false);
        }

        public void OpenContainer(Inventory container, Inventory player)
        {
            Debug.Log("OpenContainerUI OpenContainer");
            containerInventory = container;
            this.playerInventory = player;
            this.playerInventory.GetComponent<ContainerOpener>().onOpenContainerCancel += HideDisplay;

            uiCanvas.SetActive(true);
            if (containerInventoryUI != null)
            {
                containerInventoryUI.SetInventoryObject(container);
            }

            if (containerscrollRect != null)
            {
                containerscrollRect.verticalNormalizedPosition = 1f;
                Canvas.ForceUpdateCanvases();
            }
            if (playerscrollRect != null)
            {
                playerscrollRect.verticalNormalizedPosition = 1f;
                Canvas.ForceUpdateCanvases();
            }

            CharacterSheet containerCharacterSheet = container.GetComponent<CharacterSheet>();
            if (containerCharacterSheet != null)
            {
                containerName.text = containerCharacterSheet.CharacterName;
                containerImage.sprite = containerCharacterSheet.Portrait;
            }
            else
            {
                containerName.text = defaultContainerName;
                containerImage.sprite = defaultContainerImage;
            }

            CharacterSheet characterSheet = playerInventory.GetComponent<CharacterSheet>();
            playerName.text = characterSheet.CharacterName;
            playerImage.sprite = characterSheet.Portrait;
        }

        public void Close()
        {
            playerInventory.GetComponent<ContainerOpener>().Cancel();
        }

        public void TakeAll()
        {
            for (int i = 0; i < containerInventory.GetSize(); i++)
            {
                if (containerInventory.GetItemInSlot(i) != null)
                {
                    bool slotFound = playerInventory.AddToFirstEmptySlot(containerInventory.GetItemInSlot(i), containerInventory.GetNumberInSlot(i), containerInventory.GetNumberOfUsesInSlot(i));
                    if (slotFound)
                    {
                        containerInventory.RemoveFromSlot(i, containerInventory.GetNumberInSlot(i));
                    }
                }
            }
        }


        private void HideDisplay()
        {
            uiCanvas.SetActive(false);
            playerInventory.GetComponent<ContainerOpener>().onOpenContainerCancel -= HideDisplay;
        }
    }

}



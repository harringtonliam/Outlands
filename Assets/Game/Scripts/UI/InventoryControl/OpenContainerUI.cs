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

        private Container container;
        private Inventory playerInventory;

        void Start()
        {
            uiCanvas.SetActive(false);
        }

        public void OpenContainer(Inventory containerInventory, Inventory player)
        {
            Debug.Log("OpenContainerUI OpenContainer");
            container = containerInventory.GetComponent<Container>();
            this.playerInventory = player;
            this.playerInventory.GetComponent<ContainerOpener>().onOpenContainerCancel += HideDisplay;

            uiCanvas.SetActive(true);
            if (containerInventoryUI != null)
            {
                containerInventoryUI.SetInventoryObject(containerInventory);
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

            CharacterSheet containerCharacterSheet = containerInventory.GetComponent<CharacterSheet>();
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


        private void HideDisplay()
        {
            uiCanvas.SetActive(false);
            playerInventory.GetComponent<ContainerOpener>().onOpenContainerCancel -= HideDisplay;
        }
    }

}



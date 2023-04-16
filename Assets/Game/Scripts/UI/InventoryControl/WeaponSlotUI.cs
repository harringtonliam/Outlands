using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.InventoryControl;
using RPG.UI.Dragging;
using RPG.Combat;
using RPG.Control;

namespace RPG.UI.InventoryControl
{

    public class WeaponSlotUI : SelectedPlayerBasedUI, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;
        [SerializeField] Sprite selectedSprite;
        [SerializeField] Sprite normalSprite;

        // CACHE
        WeaponStore weaponStore;

        Button button;

        // LIFECYCLE METHODS
        private void OnEnable()
        {
            var player = PlayerSelector.GetFirstSelectedPlayer();
            weaponStore = player.GetComponent<WeaponStore>();
            weaponStore.storeUpdated += UpdateIcon;
            UpdateIcon();
        }

        private void Start()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (button != null)
            {
                button.onClick.AddListener(EquipWeapon);
            }
        }

        private void OnDisable()
        {
            if (weaponStore == null) return;
            weaponStore.storeUpdated -= UpdateIcon;
        }
        // PUBLIC

        public void AddItems(InventoryItem item, int number, int numberOfUses)
        {
            weaponStore.AddAction(item, index, number, true, numberOfUses);
        }

        public InventoryItem GetItem()
        {
            return weaponStore.GetAction(index);
        }

        public int GetNumber()
        {
            return weaponStore.GetNumber(index);
        }

        public int GetNumberOfUses()
        {
            return weaponStore.GetNumberOfUses(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return weaponStore.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            weaponStore.RemoveItems(index, number);
        }

        public override void SelectedPlayerChanged()
        {
            OnDisable();
            OnEnable();
        }

        // PRIVATE

        private void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
            SetSlotBackgroundImage();
        }

        private void SetSlotBackgroundImage()
        {
            Image slotImage = GetComponent<Image>();
            if (weaponStore.GetActiveWeaponIndex() == index)
            {
                slotImage.sprite = selectedSprite;
            }
            else
            {
                slotImage.sprite = normalSprite;
            }
        }

        private void EquipWeapon()
        {
            Debug.Log("weaponequiped");
            weaponStore.SetActiveWeapon(index);
        }

    }
}






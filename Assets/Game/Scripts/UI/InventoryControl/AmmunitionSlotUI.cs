using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Dragging;
using RPG.InventoryControl;
using RPG.Combat;
using RPG.Control;


namespace RPG.UI.InventoryControl
{
    public class AmmunitionSlotUI : SelectedPlayerBasedUI, IItemHolder, IDragContainer<InventoryItem>
    {
        // CONFIG DATA
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;

        // CACHE
        AmmunitionStore store;

        // LIFECYCLE METHODS
        private void OnEnable()
        {
            var player = PlayerSelector.GetFirstSelectedPlayer();
            store = player.GetComponent<AmmunitionStore>();
            store.storeUpdated += UpdateIcon;
        }

        private void OnDisable()
        {
            if (store == null) return;
            store.storeUpdated -= UpdateIcon;
        }

        // PUBLIC

        public bool AddItems(InventoryItem item, int number, int numberOfUses)
        {
            store.AddAction(item, index, number, numberOfUses);
            return true;
        }

        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }

        public int GetNumber()
        {
            return store.GetNumber(index);
        }

        public int GetNumberOfUses()
        {
            return store.GetNumberOfUses(index);
        }

        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }

        public void RemoveItems(int number)
        {
            store.RemoveItems(index, number);
        }

        public override void SelectedPlayerChanged()
        {
            OnDisable();
            OnEnable();
        }

        // PRIVATE

        void UpdateIcon()
        {
            icon.SetItem(GetItem(), GetNumber());
        }

    }
}


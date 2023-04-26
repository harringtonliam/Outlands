using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.InventoryControl
{
    public class ContainerLink : MonoBehaviour
    {
        [SerializeField] UnityEvent<Inventory, Inventory> openContainer;


        public void OpenContainer(Container container, Inventory playerInventory)
        {
            Inventory inventory = container.GetComponent<Inventory>();
            openContainer.Invoke(inventory, playerInventory);
        }

    }
}

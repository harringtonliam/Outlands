using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.Attributes;
using RPG.Core;

namespace RPG.InventoryControl
{
    [RequireComponent(typeof(Inventory))]
    public class Container : MonoBehaviour,  IRaycastable
    {
        [SerializeField] bool alwaysAvailableToRaycast = false;

        bool isOpen = false;
        ContainerLink containerLink = null;
        Inventory player = null;
        

        private void Start()
        {
            containerLink = FindFirstObjectByType<ContainerLink>();
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public RaycastableReturnValue HandleRaycast(PlayerSelector playerController)
        {
            if (!IsDead() && !alwaysAvailableToRaycast)
            {
                return RaycastableReturnValue.NoAction;
            }

            return RaycastableReturnValue.FirstPlayerCharacter;
        }

        public void HandleActivation(PlayerSelector playerController)
        {
            ContainerOpener containerOpener = playerController.transform.GetComponent<ContainerOpener>();
            if (containerOpener != null)
            {
                containerOpener.StartOpenContainer(gameObject);
            }
        }

        public void OpenContainer(Inventory playerInventory)
        {
            if (isOpen) return;

            player = playerInventory;
            isOpen = true;
            if (containerLink != null)
            {
                containerLink.OpenContainer(this, playerInventory);
            }
            
        }

        public void CloseContainer()
        {
            if (!isOpen) return;
            isOpen = false;
        }


        private bool IsDead()
        {
            Health aiHealth = GetComponent<Health>();
            if (aiHealth == null) return true;
            if (aiHealth.IsDead)
            {
                    return true;
            }
            return false;
        }
    }
}


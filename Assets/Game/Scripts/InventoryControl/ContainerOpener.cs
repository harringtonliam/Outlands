using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.InventoryControl
{
    public class ContainerOpener : MonoBehaviour, IAction
    {
        [SerializeField] float openRange = 1f;

        Container target;

        public event Action onOpenContainerCancel;

        // Update is called once per frame
        void Update()
        {
            Mover mover = GetComponent<Mover>();

            if (target != null)
            {
                mover.MoveTo(target.transform.position, 1f); ;
                if (GetIsInRange())
                {
                    mover.Cancel();
                    OpenBehaviour();
                }
            }
        }

        private void OpenBehaviour()
        {
            transform.LookAt(target.transform);
            Inventory inventory = GetComponent<Inventory>();
            target.OpenContainer(inventory);
        }

        public void StartOpenContainer(GameObject container)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = container.GetComponent<Container>(); ;
        }


        public void Cancel()
        {
            if (target != null)
            {
                target.CloseContainer();
                target = null;
            }

            GetComponent<Mover>().Cancel();
            if (onOpenContainerCancel != null)
            {
                onOpenContainerCancel();
            }
        }

        private bool GetIsInRange()
        {
            return openRange >= Vector3.Distance(target.transform.position, transform.position);
        }
    }


}


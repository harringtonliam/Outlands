using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Core
{

    public class Door : MonoBehaviour, IDoor
    {
        [SerializeField] Transform door = null;
        [SerializeField] Transform openPosition = null;
        [SerializeField] Transform closedPosition = null;
        [SerializeField] bool isLocked = false;
        [SerializeField] float doorSpeed = 0.5f;

        bool opening = false;
        bool closing = false;

        public event Action OnDoorOpen;
        public event Action OnDoorClose;

        private void Start()
        {
            closing = true;
        }


        private void Update()
        {
            if (opening)
            {
                OpenDoor();
            }
            else if (closing)
            {
                CloseDoor();
            }
            
        }

        public bool TryToOpenDoor()
        {
            if (isLocked)
            {
                return false;
            }

            closing = false;
            opening = true;
            return true;
        }

        public bool IsDoorOpen()
        {
            float distanceToOpen = Vector3.Distance(door.position, openPosition.position);
            return Mathf.Approximately(distanceToOpen, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isLocked) return;

            if (door == null  || openPosition == null) return;

            closing = false;
            opening = true;

        }


        private void OnTriggerExit(Collider other)
        {
            if (isLocked) return;

            if (door == null || closedPosition == null) return;

            opening = false;
            closing = true;

        }


        private void CloseDoor()
        {
            if (closedPosition == null) { return; }
            door.position = Vector3.MoveTowards(door.position, closedPosition.position, doorSpeed * Time.deltaTime);
            float distanceToClosed = Vector3.Distance(door.position, closedPosition.position);
            if (Mathf.Approximately(distanceToClosed, 0f))
            {
                closing = false;
                RaiseOnDoorClose();
            }
        }

        private void OpenDoor()
        {
            door.position = Vector3.MoveTowards(door.position, openPosition.position, doorSpeed * Time.deltaTime);
            float distanceToOpen = Vector3.Distance(door.position, openPosition.position);
            if (Mathf.Approximately(distanceToOpen, 0f))
            {
                opening = false;
                RaiseOnDoorOpen();
            }
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void UnLock()
        {
            isLocked = false;
        }

        private void RaiseOnDoorOpen()
        {
            if(OnDoorOpen != null) OnDoorOpen();
        }

        private void RaiseOnDoorClose()
        {
            if (OnDoorClose != null) OnDoorClose();
        }
    }

}



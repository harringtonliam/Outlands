using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;
using RPG.Core;


namespace RPG.SceneManagement
{
    public class InScenePortal : MonoBehaviour, IRaycastable
    {
        [SerializeField] Transform spawnPoint;
        [SerializeField] InScenePortal destinationPortal;
        [SerializeField] bool playerUsablePortal = true;

        Door door;

        GameObject _portalActivator;
        bool isWaitingForDoorToOpen = false;

        private void Start()
        {
            door = GetComponent<Door>();
            if (door != null )
            {
                door.OnDoorOpen += Door_OnDoorOpen;
            }
        }

        private void OnDisable()
        {
            try
            {
                if ( door != null )
                {
                    door.OnDoorOpen -= Door_OnDoorOpen;
                }
            }
            catch (Exception e)
            {

                Debug.Log("InSceenPortal exception " + e.Message);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            //if (other.tag == "Player") return;
            //UpdatePortalActivator(other.gameObject);
        }


        public void ActivatePortal(GameObject portalActivator)
        {
            if (!playerUsablePortal && portalActivator.GetComponent<PlayerController>() != null)
            {
                return;
            }

            if(portalActivator.GetComponent<PlayerController>() != null)
            {
                if(door != null && !door.IsDoorOpen() && !door.TryToOpenDoor())
                {
                    GameConsole.AddNewLine("Door is locked");
                    return;
                }
                else if(door != null && !door.IsDoorOpen())
                {
                    WaitForDoorToOpen();
                    return;
                }
                Transition(portalActivator);
            }
            else
            {
                UpdatePortalActivator(portalActivator);
            }
        }

        private void WaitForDoorToOpen()
        {
            isWaitingForDoorToOpen = true;
        }

        private void Transition(GameObject portalActivator)
        {
            UpdatePortalActivator(portalActivator);
            var playerSelector = portalActivator.GetComponent<PlayerSelector>();
            if(playerSelector.IsSelected) 
            {
                PositionTheCamera(playerSelector);
            }

        }

        //TODO remove this
        //private void TransitionOtherSelectedPlayers(GameObject portalActivator)
        //{
        //    try
        //    {
        //        var allSelectedPlayers = PlayerSelector.GetAllSelectedPlayers();
        //        int i = 1; 

        //        foreach(var player in allSelectedPlayers)
        //        {
        //            if (player.gameObject != portalActivator  && player.GetComponent<NavMeshAgent>() != null)
        //            {
        //                UpdateOtherPlayer(player, i);
        //                i++;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log("InScenePortal TransitionOtherSelectedPlayers " + ex.Message);
        //    }
        //}

        private void PositionTheCamera(PlayerSelector playerSelector)
        {
            try
            { 
                playerSelector.HandleActivation(playerSelector);
            } 
            catch (Exception ex)
            {
                Debug.Log("InScenePortal PositionCamera " + ex.Message);
            }
        }

        public void UpdatePortalActivator(GameObject portalActivator)
        {
            var mouseController = FindFirstObjectByType<MouseController>();
            var offsetDestination = destinationPortal.spawnPoint.position;
            var playerSelector = portalActivator.GetComponent<PlayerSelector>();
            if (mouseController != null  && playerSelector != null && playerSelector.Index > -1)
            {
                offsetDestination = mouseController.CalculateFormationTarget(destinationPortal.spawnPoint.position, playerSelector.Index);
            }

            portalActivator.GetComponent<NavMeshAgent>().Warp(offsetDestination);
            portalActivator.transform.rotation = destinationPortal.spawnPoint.rotation;
        }

        //private void UpdateOtherPlayer(GameObject otherPlayer, int index)
        //{
        //    //TODO:  Need to consider where the formations are kept so we don't need to find the mouse controller
        //    var mouseController = FindFirstObjectByType<MouseController>();
        //    if (mouseController == null) return;
        //    var offsetDestination = mouseController.CalculateFormationTarget(destinationPortal.spawnPoint.position, index);
        //    otherPlayer.GetComponent<NavMeshAgent>().Warp(offsetDestination);
        //    otherPlayer.transform.rotation = destinationPortal.spawnPoint.rotation;

        //}


        //private void DisablePlayerControl()
        //{
        //    GameObject player = GameObject.FindWithTag("Player");
        //    player.GetComponent<ActionScheduler>().CancelCurrentAction();
        //    player.GetComponent<PlayerController>().enabled = false;
        //}

        //private void EnablePlayerControl()
        //{
        //    try
        //    {
        //        GameObject player = GameObject.FindWithTag("Player");
        //        player.GetComponent<PlayerController>().enabled = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log("InScenePortal EnablePlayerController " + ex.Message);
        //    }
        //}

        private void Door_OnDoorOpen()
        {
            if(!isWaitingForDoorToOpen) { return; }
            isWaitingForDoorToOpen = false;
            var portalActivators = FindObjectsByType<PortalActivator>(FindObjectsSortMode.None);
            foreach(var portalActivator in portalActivators)
            {
                if(portalActivator.GetIsInRange(transform.position))
                {
                    Transition(portalActivator.gameObject);
                }
            }

        }

        public CursorType GetCursorType()
        {
                return CursorType.Pickup;
        }

        public RaycastableReturnValue HandleRaycast(PlayerSelector playerController)
        {
            if (!playerUsablePortal) return RaycastableReturnValue.NoAction;
            return RaycastableReturnValue.AllPlayerCharacters;
        }

        public void HandleActivation(PlayerSelector playerController)
        {
            PortalActivator portalActivator = playerController.GetComponent<PortalActivator>();
            if (portalActivator != null)
            {
                portalActivator.StartPortalActivation(gameObject);
            }
        }

    }


}


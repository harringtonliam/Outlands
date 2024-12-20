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
        [SerializeField] float fadeTime = 1f;

        [SerializeField] Transform spawnPoint;
        [SerializeField] InScenePortal destinationPortal;
        [SerializeField] bool playerUsablePortal = true;

        private void OnTriggerEnter(Collider other)
        {
           if (other.tag == "Player")
            {
                StartCoroutine(Transition(other.gameObject));
            }
           else
            {
                UpdatePortalActivator(other.gameObject);
            }
        }


        public void ActivatePortal(GameObject portalActivator)
        {
            if (!playerUsablePortal && portalActivator.GetComponent<PlayerController>() != null)
            {
                return;
            }

            if(portalActivator.GetComponent<PlayerController>() != null)
            {
                StartCoroutine(Transition(portalActivator));
            }
            else
            {
                UpdatePortalActivator(portalActivator);
            }
        }


        private IEnumerator Transition(GameObject portalActivator)
        {
            Fader fader = FindFirstObjectByType<Fader>();
            yield return fader.FadeOut(fadeTime);

            SavingWrapper saveingWrapper = FindFirstObjectByType<SavingWrapper>();
            DisablePlayerControl();
            UpdatePortalActivator(portalActivator);

            yield return fader.FadeIn(fadeTime);

            TransitionOtherSelectedPlayers(portalActivator);

            EnablePlayerControl();
            PositionTheCamera(portalActivator);
        }

        private void TransitionOtherSelectedPlayers(GameObject portalActivator)
        {
            try
            {
                var allSelectedPlayers = PlayerSelector.GetAllSelectedPlayers();
                int i = 1; 

                foreach(var player in allSelectedPlayers)
                {
                    if (player.gameObject != portalActivator  && player.GetComponent<NavMeshAgent>() != null)
                    {
                        UpdateOtherPlayer(player, i);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("InScenePortal TransitionOtherSelectedPlayers " + ex.Message);
            }
        }

        private void PositionTheCamera(GameObject portalActivator)
        {
            try
            {
                PlayerSelector playerSelector = portalActivator.GetComponent<PlayerSelector>();
                if (playerSelector == null) return;
                playerSelector.HandleActivation(playerSelector);
            } 
            catch (Exception ex)
            {
                Debug.Log("InScenePortal PositionCamera " + ex.Message);
            }
        }

        private void UpdatePortalActivator(GameObject portalActivator)
        {
            portalActivator.GetComponent<NavMeshAgent>().Warp(destinationPortal.spawnPoint.position);
            portalActivator.transform.rotation = destinationPortal.spawnPoint.rotation;
        }

        private void UpdateOtherPlayer(GameObject otherPlayer, int index)
        {
            //TODO:  Need to consider where the formations are kept so we don't need to find the mouse controller
            var mouseController = FindFirstObjectByType<MouseController>();
            if (mouseController == null) return;
            var offsetDestination = mouseController.CalculateFormationTarget(destinationPortal.spawnPoint.position, index);
            otherPlayer.GetComponent<NavMeshAgent>().Warp(offsetDestination);
            otherPlayer.transform.rotation = destinationPortal.spawnPoint.rotation;

        }


        private void DisablePlayerControl()
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnablePlayerControl()
        {
            try
            {
                GameObject player = GameObject.FindWithTag("Player");
                player.GetComponent<PlayerController>().enabled = true;
            }
            catch (Exception ex)
            {
                Debug.Log("InScenePortal EnablePlayerController " + ex.Message);
            }
        }

        public CursorType GetCursorType()
        {
                return CursorType.Pickup;
        }

        public RaycastableReturnValue HandleRaycast(PlayerSelector playerController)
        {
            if (!playerUsablePortal) return RaycastableReturnValue.NoAction;
            //if (Input.GetMouseButtonDown(0))
            //{
            //    PortalActivator portalActivator = playerController.transform.GetComponent<PortalActivator>();
            //    if (portalActivator != null)
            //    {
            //        portalActivator.StartPortalActivation(gameObject);
            //    }
            //}
            return RaycastableReturnValue.AllPlayerCharacters;
        }

        public void HandleActivation(PlayerSelector playerController)
        {
            PortalActivator portalActivator = playerController.transform.GetComponent<PortalActivator>();
            if (portalActivator != null)
            {
                portalActivator.StartPortalActivation(gameObject);
            }
        }

    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Control
{

    public class PlayerSelector : MonoBehaviour, IRaycastable
    {

        [SerializeField] Transform selectedVisual;
        [SerializeField] bool startSelected = false;
        [SerializeField] bool  isVisible;

        private bool isSelected;
 

        public bool IsSelected { get { return isSelected; } }

        public event Action selectedUpdated;

        public static GameObject GetFirstSelectedPlayer()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                if (player.GetComponent<PlayerSelector>().IsSelected)
                {
                    return player;
                }
            }
            return players[0];
        }

        public static List<GameObject> GetAllSelectedPlayers()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            List<GameObject> selectedPlayers = new List<GameObject>();
            foreach (var player in players)
            {
                if (player.GetComponent<PlayerSelector>().IsSelected)
                {
                    selectedPlayers.Add(player);
                }
            }
            return selectedPlayers;
        }

        private void Start()
        {
            SetSelected(startSelected);
        }

        private void OnBecameVisible()
        {
            isVisible = true;
        }

        private void OnBecameInvisible()
        {
            isVisible = false;
        }

        public void SetSelected(bool selected)
        {
            SetSelected(selected, false);
        }

        public void SetSelected(bool selected, bool controlKeyPressed)
        {
            if(selected && !controlKeyPressed)
            {
                DeSelectOtherPlayerControllers();
            }

            isSelected = selected;
            if (selectedVisual != null)
            {
                selectedVisual.GetComponent<MeshRenderer>().enabled = selected;
            }
            if (selectedUpdated != null)
            {
                selectedUpdated();
            }

        }

        private void DeSelectOtherPlayerControllers()
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in allPlayers)
            {
                player.GetComponent<PlayerSelector>().SetSelected(false);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.SelectPlayer;
        }

        public RaycastableReturnValue HandleRaycast(PlayerSelector playerController)
        {
            return RaycastableReturnValue.FirstPlayerCharacter;
        }

        public void HandleActivation(PlayerSelector playerSelector)
        {
            //TODO: Find a way of not needing find object by type;
            CameraControl.CameraController cameraController = FindObjectOfType<CameraControl.CameraController>();
            cameraController.SetPlayerToFollow(playerSelector.gameObject.transform);
        }
    }
}



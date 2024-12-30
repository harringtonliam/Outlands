using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.CameraControl;

namespace RPG.Control
{

    public class PlayerSelector : MonoBehaviour, IRaycastable
    {

        [SerializeField] Transform selectedVisual;
        [SerializeField] bool startSelected = false;
        [SerializeField] int index;

        private bool isSelected;

        public bool IsSelected { get { return isSelected; } }

        public int Index {  get { return index; } }

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

        public static void SelectAllPlayerCharacters()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                player.GetComponent<PlayerSelector>().SetSelected(true, true);
            }
        }

        private void Start()
        {
            SetSelected(startSelected);
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
            if (CameraController.Instance.IsFurtherThanMaxDistanceFromPlayer(playerSelector.gameObject.transform.position))
            {
                CameraController.Instance.SetPlayerToFollow(playerSelector.gameObject.transform);
            }
        }
    }
}



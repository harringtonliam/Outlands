using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.UI
{
    public class SelectedPlayerChangedUI : MonoBehaviour
    {
        [SerializeField]
        SelectedPlayerBasedUI[] selectedPlayerBasedUIs;

        PlayerSelector[] playerSelectors;

        private void OnEnable()
        {
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            playerSelectors = new PlayerSelector[allPlayers.Length];
            for (int i = 0; i < playerSelectors.Length; i++)
            {
                playerSelectors[i] = allPlayers[i].GetComponent<PlayerSelector>();
                playerSelectors[i].selectedUpdated += ChangeOfSelectedPlayer;
            }

            ChangeOfSelectedPlayer();
        }

        private void OnDisable()
        {
            for (int i = 0; i < playerSelectors.Length; i++)
            {
                playerSelectors[i].selectedUpdated -= ChangeOfSelectedPlayer;
            }
        }

        private void ChangeOfSelectedPlayer()
        {
            Debug.Log("Change of selected player");
            foreach (var selectedPlayerBasedUI in selectedPlayerBasedUIs)
            {
                selectedPlayerBasedUI.SelectedPlayerChanged();
            }
        }

    }

}

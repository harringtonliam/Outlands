using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.UI.InGame
{
    public class PlayerCharacterListUI : MonoBehaviour
    {
        [SerializeField]  PlayerCharacterUI playerCharacterUIPrefab = null;

        // Start is called before the first frame update
        void Start()
        {
            Redraw();
        }



        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            GameObject[] playerCharacters = GameObject.FindGameObjectsWithTag("Player");
            foreach (var playerCharacter in playerCharacters)
            {
                if (playerCharacterUIPrefab != null)
                {
                    var playerCharaceterUI = Instantiate(playerCharacterUIPrefab, transform);
                    playerCharaceterUI.SetUp(playerCharacter);
                }
            }
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.SceneManagement;
using TMPro;


namespace RPG.UI.Menus
{
    public class NewSaveGameUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField savedGameNameInput = null;

        public void SaveGame()
        {
            FindObjectOfType<SavingWrapper>().Save(savedGameNameInput.text);
            savedGameNameInput.text = string.Empty;

        }


    }

}



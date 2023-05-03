using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using TMPro;
using RPG.Core;

namespace RPG.UI.Menus
{
    public class SaveGameUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI savedGameNameText = null;
        [SerializeField] TextMeshProUGUI savedGameTimeText = null;
        [SerializeField] string saveGameConfirmationText = "Do you want to overwrite this saved game?";
        [SerializeField] string deleteGameConfirmationText = "Do you want to delete this saved game?";

        public void Setup(string savedGameName, string savedGameTime)
        {
            savedGameNameText.text = savedGameName;
            savedGameTimeText.text = savedGameTime;
        }


        public void SaveGame()
        {
            StartCoroutine(ShowSaveConfirmationDialog());
        }

        public void DeleteGame()
        {
            FindObjectOfType<SavingWrapper>().Delete(savedGameNameText.text);
        }

        public void LoadGame()
        {
            FindObjectOfType<SavingWrapper>().LoadSavedGame(savedGameNameText.text);

        }

        IEnumerator ShowSaveConfirmationDialog()
        {
            Debug.Log("ShowSaveConfirmationDialog");

            YesNoDialogUI.SetupDialog(saveGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE)
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                Debug.Log("Yes");
                FindObjectOfType<SavingWrapper>().Save(savedGameNameText.text);
            }
            else
            {
                Debug.Log("No");
            }
        }



    }

}



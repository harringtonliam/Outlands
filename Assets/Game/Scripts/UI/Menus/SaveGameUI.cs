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

        private ShowHideUI saveGameUIToClose;

        public void Setup(string savedGameName, string savedGameTime, ShowHideUI parentUI)
        {
            savedGameNameText.text = savedGameName;
            savedGameTimeText.text = savedGameTime;
            saveGameUIToClose = parentUI;
        }


        public void SaveGame()
        {
            StartCoroutine(ShowSaveConfirmationDialog());
        }

        public void DeleteGame()
        {
            StartCoroutine(ShowDeleteConfirmationDialog());
        }

        public void LoadGame()
        {
            FindObjectOfType<SavingWrapper>().LoadSavedGame(savedGameNameText.text);

        }

        IEnumerator ShowSaveConfirmationDialog()
        {
            YesNoDialogUI.SetupDialog(saveGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE)
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                Debug.Log("Yes");
                FindObjectOfType<SavingWrapper>().Save(savedGameNameText.text);
                saveGameUIToClose.ToggleUI();
            }
            else
            {
                Debug.Log("No");
            }
        }

        IEnumerator ShowDeleteConfirmationDialog()
        {
            YesNoDialogUI.SetupDialog(deleteGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE) 
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                Debug.Log("Yes");
                FindObjectOfType<SavingWrapper>().Delete(savedGameNameText.text);
            }
            else
            {
                Debug.Log("No");
            }
        }



    }

}



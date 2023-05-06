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
        [SerializeField] string loadGameConfirmationText = "Do you want to Load this game?";

        private ShowHideUI saveGameUIToClose;
        private SavingWrapper savingWrapper;

        public void Setup(string savedGameName, string savedGameTime, ShowHideUI parentUI, SavingWrapper wrapper)
        {
            savedGameNameText.text = savedGameName;
            savedGameTimeText.text = savedGameTime;
            saveGameUIToClose = parentUI;
            savingWrapper = wrapper;
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
            StartCoroutine(ShowLoadConfirmationDialogBox());
        }

        IEnumerator ShowSaveConfirmationDialog()
        {
            YesNoDialogUI.SetupDialog(saveGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE)
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                savingWrapper.Save(savedGameNameText.text);
                saveGameUIToClose.ToggleUI();
            }
        }

        IEnumerator ShowDeleteConfirmationDialog()
        {
            YesNoDialogUI.SetupDialog(deleteGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE) 
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                savingWrapper.Delete(savedGameNameText.text);
            }
        }

        IEnumerator ShowLoadConfirmationDialogBox()
        {
            YesNoDialogUI.SetupDialog(loadGameConfirmationText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE)
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                savingWrapper.LoadSavedGame(savedGameNameText.text);
            }
        }



    }

}



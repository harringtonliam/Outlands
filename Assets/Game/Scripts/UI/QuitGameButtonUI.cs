using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class QuitGameButtonUI : MonoBehaviour
    {
        [SerializeField] string quitGameText = "Do you want to quit the current game?";

        public void QuitGame()
        {
            StartCoroutine(ShowQuitGameConfirmationDialog());
        }

        IEnumerator ShowQuitGameConfirmationDialog()
        {
            YesNoDialogUI.SetupDialog(quitGameText);

            while (YesNoDialogUI.result == YesNoDialogUI.NONE)
                yield return null;

            if (YesNoDialogUI.result == YesNoDialogUI.YES)
            {
                SceneManager.LoadScene(0);
            }
        }

    }

}



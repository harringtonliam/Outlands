using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;

public class QuickSaveButtonUI : MonoBehaviour
{
    SavingWrapper savingWrapper;

    private void Start()
    {
        savingWrapper = FindFirstObjectByType<SavingWrapper>();
    }


    public void ButtonClicked()
    {
        if (savingWrapper != null)
        {
            savingWrapper.QuickSave();
        }
    }
}

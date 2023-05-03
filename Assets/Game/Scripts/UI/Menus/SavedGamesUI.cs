using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.SceneManagement;
using System;

namespace RPG.UI.Menus
{

    public class SavedGamesUI : MonoBehaviour
    {
        [SerializeField] SaveGameUI loadGameGamePrefab = null;
        [SerializeField] ShowHideUI parentUIToggle;

        SavingWrapper savingWrapper = null;

        void Start()
        {
            savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.onSaveUpated += Redraw;
            Redraw();

        }

        public void Redraw()
        {
            if (savingWrapper == null) return;

            Dictionary<string, DateTime> saveFiles = savingWrapper.ListSaveFiles();

            try
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (var saveFile in saveFiles)
                {
                    var savedGameGameUI = Instantiate(loadGameGamePrefab, transform);
                    savedGameGameUI.Setup(saveFile.Key, saveFile.Value.ToString(), parentUIToggle);
                }
            }
            catch (Exception)
            {

                Debug.Log("Unable to update SaveGameUI");
            }

        }

        private void OnDestroy()
        {
            if (savingWrapper != null)
            {
                savingWrapper.onSaveUpated -= Redraw;
            }
            
        }


    }

}



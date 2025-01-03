using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI.Menus;
using RPG.Core;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toogleKey = KeyCode.Escape;
        [SerializeField] GameObject uiCanvas = null;
        [SerializeField] bool pauseGameOnOpen = false;
        [SerializeField] bool keepOpen = false;

        public bool KeepOpen
        {
            get { return keepOpen; }
        }

        // Start is called before the first frame update
        void Start()
        {
            uiCanvas.SetActive(keepOpen);
        }

        // Update is called once per frame
        void Update()
        {
            if (InputManager.Instance.IsKeyDown(toogleKey))
            {
                ToggleUI();
            }
        }

        public void SetUiActive(bool isActive)
        {
            uiCanvas.SetActive(isActive);
            PauseOrResume();

        }

        public void ToggleUI()
        {
   
            uiCanvas.SetActive(!uiCanvas.activeSelf);
            PauseOrResume();
        }

        private void PauseOrResume()
        {
            PlayPause playPause = FindFirstObjectByType<PlayPause>();
            if( playPause == null) return;
            if (pauseGameOnOpen)
            {
                if (uiCanvas.activeSelf)
                {
                    playPause.PauseGame();
                }
                else
                {
                    playPause.ResumeGame();
                }
            }
            else
            {
                playPause.ResumeGame();
            }
        }
    }



}


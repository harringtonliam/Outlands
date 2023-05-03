using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPG.UI
{
    public class YesNoDialogUI : MonoBehaviour
    {
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;
        [SerializeField] TextMeshProUGUI dialogText;
        [SerializeField] ShowHideUI ui;

        [HideInInspector]
        public static int NONE = 0, YES = 1, NO = 2;
        [HideInInspector]
        public static int result = 0;

        private static YesNoDialogUI _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        void Start()
        {
            yesButton.onClick.AddListener(OnYesButtonClicked);
            noButton.onClick.AddListener(OnNoButtonClicked);
        }

        public static void SetupDialog(string dialogMessage)
        {
            result = 0;
            _instance.dialogText.text = dialogMessage;
            _instance.ui.ToggleUI();
        }

        public void OnYesButtonClicked()
        {
            result = YES;
            ui.ToggleUI();
        }

        public void OnNoButtonClicked()
        {
            result = NO;
            ui.ToggleUI();
        }
    }

}



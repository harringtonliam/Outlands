using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;

namespace RPG.UI
{
   


    public class TorchToggleUI : MonoBehaviour
    {
        [SerializeField] Image onBackgroundImage;
        [SerializeField] TorchSettings inGameSettings;


        Button button;

        // Start is called before the first frame update
        void Start()
        {
            ToggleBackgroundImage();

            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (button != null)
            {
                button.onClick.AddListener(Button_OnClick);
            }
        }

        private void OnEnable()
        {
            inGameSettings.SettingsUpdated += ToggleBackgroundImage;
        }

        private void OnDisable()
        {
            inGameSettings.SettingsUpdated -= ToggleBackgroundImage;
        }

        private void ToggleBackgroundImage()
        {
            onBackgroundImage.enabled = inGameSettings.IsTorchOn;
        }


        void Button_OnClick()
        {
            inGameSettings.SetIsTorchOn(!inGameSettings.IsTorchOn);
        }

    }

}


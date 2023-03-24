using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.NightVision;

namespace RPG.UI
{
    public class NightVisionToggleUI : MonoBehaviour
    {
        [SerializeField] Image onBackgroundImage;
        [SerializeField] NightVisionSettings inGameSettings;


        Button button;

        private void Start()
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

        public void Button_OnClick()
        {
            inGameSettings.SetIsNighttVisionOn(!inGameSettings.IsNightVisionOn);
        }

        private void ToggleBackgroundImage()
        {
            onBackgroundImage.enabled = inGameSettings.IsNightVisionOn;
        }


    }

}



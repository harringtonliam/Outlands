using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;
using RPG.Control;
using RPG.Core;
using TMPro;

namespace RPG.UI.InGame
{
    public class PlayerCharacterUI : MonoBehaviour
    {
        [SerializeField] RectTransform foregroundHeealthBar = null;
        [SerializeField] TextMeshProUGUI nameText = null;
        [SerializeField] TextMeshProUGUI rankText = null;
        [SerializeField] TextMeshProUGUI currentStaminaText = null;
        [SerializeField] TextMeshProUGUI maxStaminaText = null;
        [SerializeField] Image portraitImage = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Button button = null;

        GameObject playerCharacterGameObject = null;
        Health health = null;
        PlayerSelector playerSelector;

        string characterName = null;

        private void Start()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (button != null)
            {
                button.onClick.AddListener(Button_OnCLick);
            }
        }

        public void SetUp(GameObject newPlayerCharacterGameObject)
        {
            var iconImage = GetComponent<Image>();
            playerCharacterGameObject = newPlayerCharacterGameObject;

            CharacterSheet characterSheet = playerCharacterGameObject.GetComponent<CharacterSheet>();
            if (characterSheet != null)
            {
                if (characterSheet.Portrait !=null)
                {
                    portraitImage.sprite = characterSheet.Portrait;
                    portraitImage.enabled = true;
                }
                characterName = characterSheet.CharacterName;
                nameText.text = characterName;
                if (rankText != null)
                {
                    rankText.text = characterSheet.Rank;
                }
            }

            playerSelector = playerCharacterGameObject.GetComponent<PlayerSelector>();
            if (playerSelector != null)
            {
                playerSelector.selectedUpdated += ShowSelectedBackground;
                ShowSelectedBackground();
            }

            health = playerCharacterGameObject.GetComponent<Health>();
            health.healthUpdated += UpdateHealth;
            UpdateHealth();

        }

        public void Button_OnCLick()
        {
            SelectPlayer();
        }

        private void SetHealthText()
        {
            if (health == null) return;
            if (currentStaminaText != null)
            {
                currentStaminaText.text = health.HealthPoints.ToString();
            }
            if (maxStaminaText != null)
            {
                maxStaminaText.text = "/" + health.GetMaxStamina().ToString();
            }
            SetHelthPointTextColor();
        }

        private void SetHelthPointTextColor()
        {
            if (health.HealthPoints < (health.GetMaxStamina() * 0.33f))
            {
                currentStaminaText.faceColor = Color.red;
            }
            else if (health.HealthPoints < (health.GetMaxStamina() * 0.66f))
            {
                currentStaminaText.faceColor = Color.yellow;
            }
            else if (health.HealthPoints < (health.GetMaxStamina()))
            {
                currentStaminaText.faceColor = Color.green;
            }
            else
            {
                currentStaminaText.faceColor = Color.white;
            }
        }

        private void UpdateHealth()
        {
            if (health == null) return;
            SetHealthText();
            if (foregroundHeealthBar == null) return;
            Vector3 newScale = new Vector3(health.HealthPoints / health.GetMaxStamina(), 1, 1);
            foregroundHeealthBar.localScale = newScale;
        }

        private void SelectPlayer()
        {
            if (playerSelector != null)
            {
                playerSelector.SetSelected(!playerSelector.IsSelected, ControlKeyPressed());
            }
            if (playerSelector.IsSelected)
            {
                playerSelector.HandleActivation(playerSelector);
            }
        }

        private void ShowSelectedBackground()
        {
            backgroundImage.enabled = playerSelector.IsSelected;
        }

        private void OnDestroy()
        {
            if (playerSelector != null)
            {
                playerSelector.selectedUpdated -= ShowSelectedBackground;
            }
        }

        private float GetHealthFraction()
        {
            return health.HealthPoints / health.GetMaxStamina();
        }

        private bool ControlKeyPressed()
        {
            return InputManager.Instance.IsKey(KeyCode.LeftControl) || InputManager.Instance.IsKey(KeyCode.RightControl);
        }
    }



}


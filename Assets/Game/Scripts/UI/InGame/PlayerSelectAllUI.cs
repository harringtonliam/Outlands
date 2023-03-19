using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Control;


namespace RPG.UI.InGame
{
    public class PlayerSelectAllUI : MonoBehaviour
    {
        Button button = null;

        // Start is called before the first frame update
        void Start()
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }
            if (button != null)
            {
                button.onClick.AddListener(SelectAllPlayerCharacters);
            }
        }

        private void SelectAllPlayerCharacters()
        {
            PlayerSelector.SelectAllPlayerCharacters();
        }
    }

}


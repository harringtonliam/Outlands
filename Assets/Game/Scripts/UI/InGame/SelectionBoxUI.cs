using UnityEngine;
using UnityEngine.UI;
using RPG.Control;
using RPG.Core;
using System;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Unity.Cinemachine;

namespace RPG.UI.InGame
{

    public class SelectionBoxUI : MonoBehaviour
    {
        [SerializeField] private RectTransform selectionBox;
        [SerializeField] private Camera camera;

        private Vector2 startingMousePosition;

        HashSet<PlayerSelector> playersToSelect = new(5);

        // Update is called once per frame
        void Update()
        {
            HanldeSelectionBoxDrag();
        }

        private void HanldeSelectionBoxDrag()
        {
            if(selectionBox == null) { return; }

            if(InputManager.Instance.IsMouseButtonDown())
            {
                selectionBox.sizeDelta = Vector2.zero;
                playersToSelect.Clear();
                selectionBox.gameObject.SetActive(true);
                startingMousePosition = InputManager.Instance.GetMouseScreenPosition();
            }
            else if(InputManager.Instance.IsMouseButtonPressed() && !InputManager.Instance.IsMouseButtonDown())
            {
                Bounds selectectBoxBounds = ResizeSelectionBox();
                foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    Vector2 playerPosition = camera.WorldToScreenPoint(player.transform.position);
                    if (selectectBoxBounds.Contains(playerPosition) && player.TryGetComponent<PlayerSelector>(out PlayerSelector selector))
                    {
                        playersToSelect.Add(selector);
                    }
                }
            }
            else if(InputManager.Instance.IsMouseButtonUp())
            {
                if (playersToSelect.Count > 0)
                {
                    PlayerSelector.DeselectAllPlayerCharacters();
                    foreach (PlayerSelector player in playersToSelect)
                    {
                        player.SetSelected(true, true);
                    }
                }

                selectionBox.gameObject.SetActive(false);
            }
        }

        private Bounds ResizeSelectionBox()
        {
            Vector2 mousePosition = InputManager.Instance.GetMouseScreenPosition();

            float width = mousePosition.x - startingMousePosition.x;
            float height = mousePosition.y - startingMousePosition.y;

            selectionBox.anchoredPosition = startingMousePosition + new Vector2(width / 2, height / 2);
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));

            return new Bounds(selectionBox.anchoredPosition, selectionBox.sizeDelta);
        }
    }
}

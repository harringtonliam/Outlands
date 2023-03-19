using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraControl;
using UnityEngine.EventSystems;

namespace RPG.UI
{
    public class CameraMoveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] float moveDirectionY = 1f;
        [SerializeField] float moveDirectionX = 0f;
        [SerializeField] CameraController cameraController;

        bool isMousePointerOver = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            isMousePointerOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isMousePointerOver = false;
        }

        private void Update()
        {
            if (isMousePointerOver)
            {
                Vector3 moveVector = new Vector3(0, 0, 0);
                moveVector.y = moveDirectionY;
                moveVector.x = moveDirectionX;
                cameraController.MoveCamera(moveVector);
            }
        }


    }


}



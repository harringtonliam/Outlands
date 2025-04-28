//#define USE_NEW_INPUT_SYSTEM 
using RPG.CameraControl;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.Core
{
    public class InputManager : MonoBehaviour
    {

        public static InputManager Instance { get; private set; }

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogError("There is more than one input manager!" + transform + " - " + Instance.name);
                Destroy(gameObject);
            }

            Instance = this;

            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Enable();
        }

        public Vector3 GetMouseScreenPosition()
        {
#if USE_NEW_INPUT_SYSTEM
            return Mouse.current.position.ReadValue();
#else
            return Input.mousePosition;
#endif
        }

        public bool IsKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode);
        }

        public bool IsKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode);
        }

        public bool IsMouseButtonDown()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputActions.Player.Click.WasPressedThisFrame();
#else
            return Input.GetMouseButtonDown(0);
#endif
        }

        public Vector2 GetCameraMoveVector(float edgePanSize)
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
            Vector2 inputMoveDir = Vector2.zero;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                inputMoveDir.y = +1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                inputMoveDir.y = -1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                inputMoveDir.x = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                inputMoveDir.x = +1f;
            }

            inputMoveDir = inputMoveDir + GetCameraMouseMoveVector(edgePanSize);

            return inputMoveDir;
#endif
        }

        private Vector2 GetCameraMouseMoveVector(float edgePanSize)
        {
            Vector2 inputMoveDir = Vector2.zero;

            Vector2 mousePosition = GetMouseScreenPosition();

            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            if (mousePosition.x <= edgePanSize)
            {
                inputMoveDir.x = -1f;
            }

            if (mousePosition.x >= screenWidth - edgePanSize)
            {
                inputMoveDir.x = +1f;
            }

            if (mousePosition.y <= edgePanSize)
            {
                inputMoveDir.y = -1f;
            }

            if (mousePosition.y >= screenHeight - edgePanSize)
            {
                inputMoveDir.y = 1f;
            }

            return inputMoveDir;
        }




        public float GetCameraRoatateAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
            float rotateAmount = 0f;

            //Debug.Log("get rotate amount" + Input.GetAxis("Mouse X").ToString());
            if (Input.GetMouseButton(1))
            {
                if (Input.GetAxis("Mouse X") > Mathf.Epsilon || Input.GetAxis("Mouse X") < Mathf.Epsilon)
                {
                    rotateAmount = Input.GetAxis("Mouse X");
                }
                
            }

            return rotateAmount;
           
#endif
        }

        public float GetCameraZoomAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
            float zoomAmount = 0f;

            if(Input.mouseScrollDelta.y > 0)
            {
                zoomAmount = -1f;
            }
            if(Input.mouseScrollDelta.y < 0)
            {
                zoomAmount = +1f;
            }

            return zoomAmount;
#endif

        }

    }

}



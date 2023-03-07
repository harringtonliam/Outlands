using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using RPG.Core;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float rotationSpeed = 100f;
        [SerializeField] float zoomSpeed = 5f;
        [SerializeField] float minFollowYOffset = 2f;
        [SerializeField] float maxFollowYOffset = 15f;
        [SerializeField] float zoomIncreaseAmount = 1f;
        [SerializeField] CinemachineVirtualCamera virtualCamera;


        private Vector3 targetFollowOffset;
        private CinemachineTransposer cinemachineTransposer;

        Transform playerToFollow;
        float currentXPositionOffset = 0f;
        float cuurentZPositionOffset = 0f;


        private void Start()
        {
            cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
            targetFollowOffset = cinemachineTransposer.m_FollowOffset;

        }



        // Update is called once per frame
        void Update()
        {
            HandleMovement();

            HandleRotation();

            HandleZoom();

        }

        public void SetPlayerToFollow(Transform player)
        {
            playerToFollow = player;
            transform.position = playerToFollow.position;
        }

        private void ResetPositionOffsets()
        {
            currentXPositionOffset = 0f;
            cuurentZPositionOffset = 0f;
        }

        private void HandleZoom()
        {
            targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
            targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, minFollowYOffset, maxFollowYOffset);
            cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
        }

        private void HandleRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);

            rotationVector.y = InputManager.Instance.GetCameraRoatateAmount();

            transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        }

        private void HandleMovement()
        {
            Vector3 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();

            Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;

            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }
    }


}



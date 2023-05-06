using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using RPG.Core;
using RPG.Control;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float rotationSpeed = 100f;
        [SerializeField] float rotationFactor = 50f;
        [SerializeField] float zoomSpeed = 5f;
        [SerializeField] float minFollowYOffset = 2f;
        [SerializeField] float maxFollowYOffset = 15f;
        [SerializeField] float zoomIncreaseAmount = 1f;
        [SerializeField] float maxDistanceFromSelectedPlayer = 10f;
        [SerializeField] CinemachineVirtualCamera virtualCamera;


        private Vector3 targetFollowOffset;
        private CinemachineTransposer cinemachineTransposer;

        private static CameraController _instance;

        public static CameraController Instance { get { return _instance; } }

        Transform playerToFollow;

        private void Awake()
        {
            {
                if (_instance != null && _instance != this)
                {
                    Debug.LogError("There is more than one Camera Controller");
                    Destroy(this.gameObject);
                }
                else
                {
                    _instance = this;
                }
            }
        }

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

        public void MoveCamera(Vector3 moveDirection)
        {
            float currentDistanceFromSelectedPlayer = Vector3.Distance(PlayerSelector.GetFirstSelectedPlayer().transform.position, transform.position);

            Vector3 moveVector = transform.forward * moveDirection.y + transform.right * moveDirection.x;
            float newDistanceFromSelectedPlayer = Vector3.Distance(PlayerSelector.GetFirstSelectedPlayer().transform.position, transform.position + moveVector);

            if ((currentDistanceFromSelectedPlayer >= maxDistanceFromSelectedPlayer)  && (newDistanceFromSelectedPlayer > currentDistanceFromSelectedPlayer))
            {
                return;
            }

            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        public bool IsFurtherThanMaxDistanceFromPlayer(Vector3 playerPosition)
        {
            if (Vector3.Distance(playerPosition, transform.position) >= maxDistanceFromSelectedPlayer)
            {
                return true;
            }
            return false;
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

            rotationVector.y = InputManager.Instance.GetCameraRoatateAmount() * rotationFactor;

            transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        }

        private void HandleMovement()
        {
            MoveCamera(InputManager.Instance.GetCameraMoveVector());
        }
    }


}



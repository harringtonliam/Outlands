using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using RPG.Core;
using RPG.Control;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineCamera cinemachineCamera;
        [SerializeField] CameraConfig cameraConfig;


        private Vector3 startingFollowOffset;
        private CinemachineFollow cinemachineFollow;
        private Rigidbody rigidbody;

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

            if (!cinemachineCamera.TryGetComponent(out cinemachineFollow))
            {
                Debug.LogError("Cinemachine Camera did not have a CineMachineFollow. Zoom functionality will not work!");
            }
            startingFollowOffset = cinemachineFollow.FollowOffset;
            if (!TryGetComponent<Rigidbody>(out rigidbody))
            {
                Debug.LogError("Cinemachine Camera did not have a RigidBody. Move functionality will not work!");
            };
        }



        // Update is called once per frame
        void Update()
        {
            HandlePanning();

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

            Vector3 moveVector = transform.forward * moveDirection.y + transform.right * moveDirection.x;

            moveVector = moveVector * cameraConfig.PanSpeed;// * Time.deltaTime;
            rigidbody.linearVelocity = new Vector3(moveVector.x, moveVector.y, 0);

        }


        private void HandleZoom()
        {
            startingFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * cameraConfig.zoomIncreaseAmount;
            startingFollowOffset.y = Mathf.Clamp(startingFollowOffset.y, cameraConfig.MinFollowYOffset, cameraConfig.MaxFollowYOffset);
            cinemachineFollow.FollowOffset = Vector3.Slerp(cinemachineFollow.FollowOffset, startingFollowOffset, Time.deltaTime * cameraConfig.ZoomSpeed);
        }

        private void HandleRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);

            rotationVector.y = InputManager.Instance.GetCameraRoatateAmount() * cameraConfig.RotationFactor;

            transform.eulerAngles += rotationVector * cameraConfig.RotationSpeed * Time.deltaTime;
        }

        private void HandlePanning()
        {
            MoveCamera(InputManager.Instance.GetCameraMoveVector(cameraConfig.EdgePanSize));
        }



    }


}



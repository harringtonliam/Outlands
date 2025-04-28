using UnityEngine;

namespace RPG.CameraControl
{
    [System.Serializable]
    public class CameraConfig
    {
        [field: SerializeField] public bool EnableEdgePan { get; private set; } = true;
        //[field: SerializeField] public float MousePanSpeed { get; private set; } = 5;
        [field: SerializeField] public float EdgePanSize { get; private set; } = 50;

        //[field: SerializeField] public float KeyboardPanSpeed { get; private set; } = 5;
        [field: SerializeField] public float PanSpeed = 10f;

        [field: SerializeField] public float RotationSpeed { get; private set; } = 1f;
        [field: SerializeField] public float RotationFactor { get; private set; } = 50f;

        [field: SerializeField] public float ZoomSpeed { get; private set; } = 1f;
        [field: SerializeField] public float MinZoomDistance { get; private set; } = 7.5f;
        [field: SerializeField] public float MinFollowYOffset = 2f;
        [field: SerializeField] public float MaxFollowYOffset = 15f;
        [field: SerializeField] public float zoomIncreaseAmount { get; private set; } = 1f;


        //[SerializeField] float moveSpeed = 10f;
        //[SerializeField] float rotationSpeed = 100f;
        //[SerializeField] float rotationFactor = 50f;
        //[SerializeField] float zoomSpeed = 5f;

        //[SerializeField] float zoomIncreaseAmount = 1f;
        //[SerializeField] float maxDistanceFromSelectedPlayer = 10f;

    }
}



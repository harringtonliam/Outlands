using RPG.WeatherControl;
using System;
using UnityEngine;

namespace RPG.Environment
{
    public class CameraEnvironmentController : MonoBehaviour
    {

        [SerializeField] float interiorLoactionHeight = -50f;
 


        private float storedPositionY;

        public event Action onCameraMovedIndoorsOrOutDoors;


        public bool IsCameraAtInteriorLocation()
        {
            return transform.position.y <= interiorLoactionHeight;
        }

        // Start is called before the first frame update
        void Start()
        {
            storedPositionY = transform.position.y;
        }

        private void Update()
        {
            if (Mathf.Abs(storedPositionY - transform.position.y) > 20)
            {
                storedPositionY = transform.position.y;
                CameraMovedIndoorsorOutDoors();
            }
        }

        private void CameraMovedIndoorsorOutDoors()
        {
            if(onCameraMovedIndoorsOrOutDoors != null) {
                onCameraMovedIndoorsOrOutDoors();
            }
        }

    }

}



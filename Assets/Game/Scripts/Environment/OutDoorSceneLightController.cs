using UnityEngine;
using System;

namespace RPG.Environment
{
    public class OutDoorSceneLightController : MonoBehaviour
    {
        [SerializeField] Light[] interiorLights;
        [SerializeField] Light[] naturalLights;
        [SerializeField] CameraEnvironmentController cameraEnvironmentController;


        private void Start()
        {
            cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors += UpdateInteriorLighting;
            UpdateInteriorLighting();
        }


        private void OnDisable()
        {
            try
            {
                cameraEnvironmentController.onCameraMovedIndoorsOrOutDoors -= UpdateInteriorLighting;
            }
            catch (System.Exception e)
            {

                Debug.Log(String.Format("OutDoorSceneLightController {0}", e.Message));
            }
        }

        public void UpdateInteriorLighting()
        {
            foreach(Light light in interiorLights)
            {
                light.gameObject.SetActive(cameraEnvironmentController.IsCameraAtInteriorLocation());
            }
            foreach(Light light in naturalLights)
            {
                light.gameObject.SetActive(!cameraEnvironmentController.IsCameraAtInteriorLocation());
            }
        }

    }




}



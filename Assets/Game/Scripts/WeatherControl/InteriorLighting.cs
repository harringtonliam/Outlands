using System;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.WeatherControl
{
    public class InteriorLighting : MonoBehaviour
    {
        [SerializeField] Light[] interiorLights;
        [SerializeField] Light[] naturalLights;
        [SerializeField] float interiorLoactionHeight = -50f;

        private float storedPositionY;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            storedPositionY = transform.position.y;
            SetLighting();
        }



        // Update is called once per frame
        void Update()
        {
            if (Mathf.Abs(storedPositionY - transform.position.y) > 20)
            {
                storedPositionY = transform.position.y;
                SetLighting();
            }
        }

        private void SetLighting()
        {
            foreach(Light light in interiorLights)
            {
                light.gameObject.SetActive(IsInteriorLocation());
            }
        }

        private bool IsInteriorLocation()
        {
            if (transform.position.y <= interiorLoactionHeight)
            {
                return true;
            }
            return false;
        }
    }

}



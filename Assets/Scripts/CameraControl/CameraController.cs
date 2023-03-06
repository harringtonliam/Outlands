using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 1f;

        Transform selectedPlayer;

        // Start is called before the first frame update
        void Start()
        {
            selectedPlayer = GameObject.FindWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = selectedPlayer.position;
            CameraRotation();
        }

        private void CameraRotation()
        {
            Vector3 rotationVector = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.Q))
            {
                rotationVector.y = +1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotationVector.y = -1f;
            }
            transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
        }
    }


}



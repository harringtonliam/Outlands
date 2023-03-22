using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Core
{
    public class TorchSettings : MonoBehaviour
    {
        bool isTorchOn = false; 

        public bool IsTorchOn {  get { return isTorchOn; } }

        public event Action SettingsUpdated;

        public void SetIsTorchOn(bool torchOn)
        {
            Debug.Log("SetIsTorchOn " + torchOn.ToString());

            isTorchOn = torchOn;

            if (SettingsUpdated != null)
            {
                SettingsUpdated();
            }
        }

    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Core;

public class CharacterTorch : MonoBehaviour
{
    [SerializeField] Light torchLight;

    TorchSettings torchSettings;

    // Start is called before the first frame update
    void Awake()
    {
        torchSettings = FindFirstObjectByType<TorchSettings>();
    }

    private void OnEnable()
    {
        torchSettings.SettingsUpdated += TorchOnOff;
        TorchOnOff();
    }

    private void OnDisable()
    {
        torchSettings.SettingsUpdated -= TorchOnOff;
    }

    private void TorchOnOff()
    {
        torchLight.enabled = torchSettings.IsTorchOn;
    }


}

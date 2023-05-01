using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core

{
    public class ParameterControl : MonoBehaviour
    {
        [SerializeField] GameParameter gameParameter;

        private static ParameterControl _instance;

        Dictionary<string, GlobalParameter> parameterDictionary;

        void Awake()
        {
            Debug.Log("Game ParameterControl awake");
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                SetupDictionary();
            }
        }

        public static string GetStringValue(string parameterName)
        {
            GlobalParameter globalParameter = _instance.FindParameter(parameterName);
            if (globalParameter == null) return string.Empty;
            return globalParameter.stringValue;
        }

        public static int GetIntValue(string parameterName)
        {
            GlobalParameter globalParameter = _instance.FindParameter(parameterName);
            if (globalParameter == null) return 0;
            return globalParameter.intValue;
        }

        public static float GetFloatValue(string parameterName)
        {
            GlobalParameter globalParameter = _instance.FindParameter(parameterName);
            if (globalParameter == null)
                return 0f;
            return globalParameter.floatValue;
        }

        private GlobalParameter FindParameter(string name)
        {
            GlobalParameter globalParameter;
            if (parameterDictionary.TryGetValue(name, out globalParameter))
            {
                return globalParameter;
            }
            return null;
        }

        private void SetupDictionary()
        {
            parameterDictionary = new Dictionary<string, GlobalParameter>();

            foreach (var globalParameter in gameParameter.GlobalParameters)
            {
                bool addedOk = parameterDictionary.TryAdd(globalParameter.parameterName, globalParameter);
                if (!addedOk)
                {
                    Debug.LogError("Failed to add global parameter because there is a duplicate " + globalParameter.parameterName);
                }
            }
        }
    }
}



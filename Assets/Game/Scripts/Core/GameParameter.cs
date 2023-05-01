using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Core
{
    [CreateAssetMenu(fileName = "GameParameter", menuName = "Settings/Make New Game Parameters", order = 0)]
    public class GameParameter : ScriptableObject
    {
        [SerializeField] GlobalParameter[] globalParameters;

        public GlobalParameter[] GlobalParameters { get { return globalParameters; } }   

    }

    public enum GameParameterType
    {
        StringSetting,
        IntSetting,
        FloatSetting
    }

    [Serializable]
    public class GlobalParameter
    {
        public string parameterName;
        public GameParameterType gameParameterType;
        public string stringValue;
        public int intValue;
        public float floatValue;

    }
}



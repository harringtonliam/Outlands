using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Movement
{


    [CreateAssetMenu(fileName = "Formation", menuName = "Movement/Make New Formation", order = 0)]
    public class Formation : ScriptableObject
    {
        [SerializeField] FormationOffset[] formationOffsets;


        [Serializable]
        public struct FormationOffset
        {
            public float OffsetX;
            public float OffsetY;
            public float OffsetZ;
        }


        public FormationOffset GetForamtionOffSet(int index)
        {
            if (index < formationOffsets.Length)
            {
                return formationOffsets[index];
            }

            return ZeroFormationOffset();
        }

        private static FormationOffset ZeroFormationOffset()
        {
            FormationOffset zeroFormationOffset = new FormationOffset();
            zeroFormationOffset.OffsetX = 0f;
            zeroFormationOffset.OffsetY = 0f;
            zeroFormationOffset.OffsetZ = 0f;
            return zeroFormationOffset;
        }

        public Vector3 GetFormationOffsetVector3(int index)
        {
            if (index < formationOffsets.Length)
            {
                return ConvertToVector3(formationOffsets[index]);
            }
            
            return ConvertToVector3(ZeroFormationOffset());
        }

        private Vector3 ConvertToVector3(FormationOffset formationOffset)
        {
            return  new Vector3(formationOffset.OffsetX, formationOffset.OffsetY, formationOffset.OffsetZ);
        }
    }

}

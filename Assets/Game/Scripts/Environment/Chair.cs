using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Scenery
{
    public class Chair : MonoBehaviour
    {
        [SerializeField] Transform sitTransform;
        [SerializeField] string animationTrigger;
        [SerializeField] OccupiedBehaviour[] occupiedBehaviours;

        [Serializable]
        public struct OccupiedBehaviour
        {
            public GameObject gameObject;
            public bool isVisibleWhenOccupied;
        }


        private bool isOccupied = false;

        public bool IsOccupied {  get { return isOccupied; } set { isOccupied = value; } }

        public Transform SitTransform { get { return sitTransform; } }
        public string AnimationTrigger { get { return animationTrigger; } }

        private void Start()
        {
            ProcessOccuipedBehaviours();
        }


        public void MakeChairOccuiped(bool newIsOccuiped)
        {
            this.isOccupied = newIsOccuiped;
            ProcessOccuipedBehaviours();

        }

        private void ProcessOccuipedBehaviours()
        {
            foreach (var occupiedBehaviour in occupiedBehaviours)
            {
                occupiedBehaviour.gameObject.SetActive(isOccupied == occupiedBehaviour.isVisibleWhenOccupied);
            }
        }
    }

}



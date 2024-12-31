using System;
using UnityEngine;

namespace RPG.FurnitureControl
{
    public class Furniture : MonoBehaviour
    {
        [SerializeField] Transform occupiedTransform;
        [SerializeField] string animationTrigger;
        [SerializeField] OccupiedBehaviour[] occupiedBehaviours;

        [Serializable]
        public struct OccupiedBehaviour
        {
            public GameObject gameObject;
            public bool isVisibleWhenOccupied;
        }


        private bool isOccupied = false;

        public bool IsOccupied { get { return isOccupied; } set { isOccupied = value; } }

        public Transform OccupiedTransform { get { return occupiedTransform; } }
        public string AnimationTrigger { get { return animationTrigger; } }

        private void Start()
        {
            ProcessOccuipedBehaviours();
        }


        public void MakeFurnitureOccuiped(bool newIsOccuiped)
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



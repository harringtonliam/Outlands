using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.FurnitureControl;
using UnityEngine.AI;

namespace RPG.Control
{
    public class FurnitureController : MonoBehaviour
    {
        [SerializeField] float furinturePositionTolerance = 0.9f;
        [SerializeField] float seatedColiderHeightProportion = 0.75f;


        bool isOnFurniture = false;
        bool isActionHappening = false;

        Vector3 standPosition;
        Furniture currentcurrentFurniture;
        ColliderController colliderController;


        public bool IsOnFurniture { get { return isOnFurniture; } }
        public bool IsActionHappening { get { return isActionHappening; } }
        public float ChairPositionTolerance { get { return furinturePositionTolerance; } }


        private void Start()
        {
            colliderController = GetComponent<ColliderController>();
        }

        public bool IsInteractingWithFurniture()
        {
            return isOnFurniture || isActionHappening;
        }


        public void OccupyFurniture(Furniture targetFurniture)
        {
            if (isActionHappening) return;
            if (targetFurniture.IsOccupied) return;

            isActionHappening = true;
            GetComponent<NavMeshAgent>().enabled = false;
            standPosition = transform.position;
            transform.position = targetFurniture.OccupiedTransform.position;
            transform.rotation = targetFurniture.OccupiedTransform.rotation;


            //Debug.Log("Triggering sit Animation");
            string annimationTrigger = targetFurniture.AnimationTrigger;
            GetComponent<Animator>().SetTrigger(annimationTrigger);
            //colliderController.ResizeCollider(seatedColiderHeightProportion);
            isOnFurniture = true;
            isActionHappening = false;
            currentcurrentFurniture = targetFurniture;
            currentcurrentFurniture.MakeFurnitureOccuiped(true);
        }

        public void GetOffFurniture()
        {
            if (isActionHappening) return;

            isActionHappening = true;
            //Debug.Log("Triggering stand Animation");
            GetComponent<Animator>().SetTrigger("stand");
            //colliderController.ResetCollider();
            transform.position = standPosition;
            isOnFurniture = false;
            currentcurrentFurniture.MakeFurnitureOccuiped(false);
            GetComponent<NavMeshAgent>().enabled = true;
            isActionHappening = false;
        }


    }

}



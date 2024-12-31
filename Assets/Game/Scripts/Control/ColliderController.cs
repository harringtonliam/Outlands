using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{

    public class ColliderController : MonoBehaviour
    {
        CapsuleCollider capsuleCollider;

        float startColliderderHeight;
        Vector3 startColliderCenter;

        public float StartColliderHeight {  get { return startColliderderHeight; } }
        public Vector3 StartColliderCenter {  get { return startColliderCenter; } }


        // Start is called before the first frame update
        void Start()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            startColliderderHeight = capsuleCollider.height;
            startColliderCenter = capsuleCollider.center;
        }


        public void ResetCollider()
        {
            capsuleCollider.height = startColliderderHeight;
            capsuleCollider.center = startColliderCenter;
        }


        public void ResizeCollider(float newHeightProportion)
        {
            capsuleCollider.height = capsuleCollider.height*newHeightProportion;
            Vector3 newCentre = new Vector3(startColliderCenter.x, startColliderCenter.y * newHeightProportion, startColliderCenter.z);
            capsuleCollider.center = newCentre;
        }

    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class VisibleIndicator : MonoBehaviour
    {
        [SerializeField] bool isVisible;


        public bool IsVisible {  get { return isVisible; } }

        private void OnBecameInvisible()
        {
            isVisible = false;
        }

        private void OnBecameVisible()
        {
            isVisible = true;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{


    public class IsVisible : MonoBehaviour
    {
        [SerializeField] bool Visible;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnBecameInvisible()
        {
            Visible = false;
        }

        private void OnBecameVisible()
        {
            Visible = true;
        }
    }

}

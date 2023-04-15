using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class SelectedPlayerBasedUI : MonoBehaviour
    {

        /// <summary>
        /// Implment the things to do when the selected player is changed
        /// </summary>
        public virtual void SelectedPlayerChanged()
        {
            Debug.Log("Selected Player Changed");
        }
    }

}



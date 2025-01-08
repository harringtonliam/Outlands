using UnityEngine;

namespace RPG.Buildings
{
    [RequireComponent(typeof(DestinationSettings))]
    public class DestinationSettingsArray : MonoBehaviour
    {
        [SerializeField] GameObject[] destiationObjects;

        public GameObject[] DestiationObjects {  get { return destiationObjects; } }

        public GameObject GetRandomDestinationObject()
        {
            if (destiationObjects.Length <= 0) return null;

            int index = Random.Range(0, destiationObjects.Length);
            return destiationObjects[index];
        }


    }

}



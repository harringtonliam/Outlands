using UnityEngine;

namespace RPG.Buildings
{
    public class HouseSettings : MonoBehaviour
    {
        [SerializeField] GameObject[] dayTimeDestinations;
        [SerializeField] GameObject[] nightTimeDestinations;


        public GameObject[] DayTimeDestinations { get { return dayTimeDestinations; } }
        public GameObject[] NightTimeDestinations { get { return NightTimeDestinations; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}



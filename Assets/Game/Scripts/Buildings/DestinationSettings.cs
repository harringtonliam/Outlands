
using UnityEngine;

namespace RPG.Buildings
{
    public class DestinationSettings : MonoBehaviour
    {
        [SerializeField] float distanceTolerance = 1f;
        [SerializeField] bool generateRandomDestination = false;
        [SerializeField] float randomDestinationLifeTime = 60f;
        [SerializeField] float randomDestinationRange = 5f;


        public float RandomDestiationLifeTime { get { return randomDestinationLifeTime; } }
        public float DistanceTolerance {  get { return distanceTolerance; } }

        public struct DestintationInfo
        {
            public GameObject destinationInfoObject;
            public Vector2 destinattionInfoPostition;
        }    




        public DestintationInfo GetDestinationPosition()
        {
            DestintationInfo destintationInfo = new DestintationInfo();
            destintationInfo.destinattionInfoPostition = new Vector3();
            destintationInfo.destinationInfoObject = null;

            if (!generateRandomDestination)
            {
                destintationInfo.destinattionInfoPostition = transform.position;
                return destintationInfo;
            }

            float xOffSet = Random.Range(randomDestinationRange * -1, randomDestinationRange);
            float zOffSet = Random.Range(randomDestinationRange * -1, randomDestinationRange);
            var postionOffset = new Vector3(xOffSet, 0, zOffSet);

            destintationInfo.destinattionInfoPostition = transform.position + postionOffset;

            return destintationInfo;
        }

    }


}



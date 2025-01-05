
using UnityEngine;

namespace RPG.Buildings
{
    public class DestinationSettings : MonoBehaviour
    {
        [SerializeField] float distanceTolerance = 1f;
        [SerializeField] bool generateRandomDestination = false;
        [SerializeField] float randomDestinationLifeTime = 60f;
        [SerializeField] float randomDestinationXRange = 5f;
        [SerializeField] float randomDestinationZRange = 5f;


        public float RandomDestiationLifeTime { get { return randomDestinationLifeTime; } }
        public float DistanceTolerance {  get { return distanceTolerance; } }

        public struct DestintationInfo
        {
            public GameObject destinationInfoObject;
            public Vector3 destinattionInfoPostition;
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

            float xOffSet = Random.Range(randomDestinationXRange * -1, randomDestinationXRange);
            float zOffSet = Random.Range(randomDestinationZRange * -1, randomDestinationZRange);
            Debug.Log("Destination settings " + xOffSet + " zOffset=" + zOffSet);
            var postionOffset = new Vector3(xOffSet, 0, zOffSet);

            destintationInfo.destinattionInfoPostition = transform.position + postionOffset;
            Debug.Log("Destination settings result " + destintationInfo.destinattionInfoPostition.x + " " + destintationInfo.destinattionInfoPostition.z);

            return destintationInfo;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, new Vector3(randomDestinationXRange,1f, randomDestinationZRange));
        }

    }


}



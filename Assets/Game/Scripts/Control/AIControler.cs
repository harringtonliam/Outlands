using RPG.Attributes;
using RPG.Buildings;
using RPG.Combat;
using RPG.Core;
using RPG.FurnitureControl;
using RPG.Movement;
using RPG.GameTime;
using System;
using UnityEngine;
using System.Collections;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] AIRelationship aIRelationship = AIRelationship.Hostile;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] float aggrevationCoolDownTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointPauseTime = 2f;
        [Range(0f, 1f)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] GameObject combatTargetGameObject;
        [SerializeField] GameObject destination;
        [SerializeField] GameObject defaultWorkDestintation;
        [SerializeField] int startHourOfWorkDay = 9;
        [SerializeField] int endHourOfWorkDay = 18;
        [SerializeField] HouseSettings homeHouse;
        [SerializeField] int homeHouseIndex = 0;
        

        Mover mover;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        float timeAtWaypoint = Mathf.Infinity;
        float timeWithDestination = Mathf.Infinity;
        int currentWaypointIndex = 0;
        FurnitureController furnitureController;
        GameTimeContoller gameTimeController;
        DestinationSettings.DestintationInfo cachedDestinationInfo;


        public AIRelationship AIRelationship
        {
            get{ return aIRelationship;}
        }

        private void Awake()
        {
             mover = GetComponent<Mover>();
            if (aIRelationship == AIRelationship.Hostile)
            {
                IsAggrevated();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            gameTimeController = FindFirstObjectByType<GameTimeContoller>();

            furnitureController = GetComponent<FurnitureController>();

            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
            timeWithDestination += Time.deltaTime;



            if (GetComponent<Health>().IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithSuspicion()) return;
            if (InteractWithPatrolPath()) return;
            if (InteractWithDestination()) return;
            if (InteractWithWorkDestination()) return;
            if (InteractWithHomeHouse()) return;
            if (InteractWithGuardPosition()) return;
        }



        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        public void SetChaseDistance(float newChaseDistance)
        {
            chaseDistance = newChaseDistance;
        }

        public void SetPatrolPath(PatrolPath newPatrolPath)
        {
            patrolPath = newPatrolPath;
            currentWaypointIndex = 0;
        }

        public void SetDesitination(GameObject newDesitination)
        {
            destination = newDesitination;
        }

        public void SetCombatTarget(GameObject target)
        {
            combatTargetGameObject = target;
        }

        private bool InteractWithPatrolPath()
        {
            if (patrolPath == null) return false;

            timeAtWaypoint += Time.deltaTime;

            if (AtWaypoint())
            {
                timeAtWaypoint = 0;
                CycleWaypoint();
            }

            if (currentWaypointIndex < 0)
            {
                patrolPath = null;
                return false;
            }

            GetOffFurnitureIfNeeded();

            if (timeAtWaypoint > waypointPauseTime)
            {
                mover.StartMovementAction(GetCurrentWaypoint(), patrolSpeedFraction);
            }

            return true;
        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWayPoint <= waypointTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool AtPosition(Vector3 positionToCheck)
        {
            return AtPosition(positionToCheck, waypointTolerance);
        }

        private bool AtPosition(Vector3 positionToCheck, float positionTolerance)
        {
            float distanceToPosition = Vector3.Distance(transform.position, positionToCheck);
            if (distanceToPosition <= positionTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsDestiationFurniture(Transform destination)
        {
            if (destination == null) return false;

            if (destination.GetComponent<Furniture>() != null)
            {
                return true;
            }
            if (destination.parent != null && destination.parent.GetComponent<Furniture>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetOffFurnitureIfNeeded()
        {
            if (furnitureController.IsOnFurniture)
            {
                furnitureController.GetOffFurniture();
            }
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        
        private bool InteractWithSuspicion()
        {
            if (timeSinceLastSawPlayer < suspicionTime )
            {
                ActionScheduler actionSchduler = GetComponent<ActionScheduler>();
                actionSchduler.CancelCurrentAction();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InteractWithDestination()
        {
            if(destination == null) return false;
            DestinationProcessing(destination.gameObject);

            return true;
        }

        private bool InteractWithWorkDestination()
        {
            if (defaultWorkDestintation == null) { return false; }
            if (gameTimeController.CurrentLocalHour < startHourOfWorkDay || gameTimeController.CurrentLocalHour >= endHourOfWorkDay) return false;
            DestinationProcessing(defaultWorkDestintation);

            return true;
        }

        private void DestinationProcessing(GameObject desination)
        {
            GameObject objectToGoTo = desination;
            Vector3 positionToGoTo = objectToGoTo.transform.position;
            var positionTolerance = waypointTolerance;

            DestinationSettings destinationSettings = desination.GetComponent<DestinationSettings>();
            if (destinationSettings != null)
            {
                positionTolerance = destinationSettings.DistanceTolerance;
                if (timeWithDestination >= destinationSettings.RandomDestiationLifeTime)
                {
                    timeWithDestination = 0f;
                    cachedDestinationInfo = destinationSettings.GetDestinationPosition();
                }
                objectToGoTo = cachedDestinationInfo.destinationInfoObject;
                positionToGoTo = cachedDestinationInfo.destinattionInfoPostition;
            }


            //Debug.Log("positionToGoTo  " + gameObject.name + " " + positionToGoTo.x + " " + positionToGoTo.z);
            if (AtPosition(positionToGoTo, positionTolerance))
            {
                bool isItFurniture = InteractWithFurniture(objectToGoTo);
                if(!isItFurniture)
                {
                    StartCoroutine(RotateToFaceDestination());
                }
            }
            else
            {
                GetOffFurnitureIfNeeded();
                mover.StartMovementAction(positionToGoTo, patrolSpeedFraction);
            }
        }

        private IEnumerator RotateToFaceDestination()
        {
            yield return new WaitForSeconds(1f);
            if (cachedDestinationInfo.destinationInfoObject != null)
            {
                mover.Cancel();
                this.transform.rotation = cachedDestinationInfo.destinationInfoObject.transform.rotation;
            }
        }

        private bool InteractWithHomeHouse()
        {
            if (homeHouse == null) return false;
            var positionToGoTo = homeHouse.DayTimeDestinations[homeHouseIndex];
            if (homeHouse.IsNightTime())
            {
                positionToGoTo = homeHouse.NightTimeDestinations[homeHouseIndex];
            }


            if (AtPosition(positionToGoTo.transform.position))
            {
                bool isItFurniture = InteractWithFurniture(positionToGoTo);
            }
            else
            {
                GetOffFurnitureIfNeeded();
                mover.StartMovementAction(positionToGoTo.transform.position, patrolSpeedFraction);
            }
            return true;
        }

        

        private bool InteractWithFurniture(GameObject destination)
        {
            if(destination == null) return false;
            if (!AtPosition(destination.transform.position)) return false;
            if(!IsDestiationFurniture(destination.transform)) return false;
            if(furnitureController.IsInteractingWithFurniture()) return false;
            var furniture = destination.GetComponent<Furniture>();
            mover.Cancel();
            furnitureController.OccupyFurniture(furniture);
            return true;
        }

        private bool InteractWithGuardPosition()
        {

            GetOffFurnitureIfNeeded();

            mover.StartMovementAction(guardPosition, patrolSpeedFraction);
            return true;
        }

        private bool InteractWithCombat()
        {

            Fighting fighter = GetComponent<Fighting>();
            if (combatTargetGameObject == null)
            {
                fighter.Cancel();
                return false;
            }
            if (IsAggrevated() && fighter.CanAttack(combatTargetGameObject))
            {
                GetOffFurnitureIfNeeded();
                timeSinceLastSawPlayer = 0;
                fighter.Attack(combatTargetGameObject);
                AggrevateNearbyEnemies();
                return true;
            }
            else
            {
                fighter.Cancel();
                return false;
            }
        }

        private void AggrevateNearbyEnemies()
        {
            if (aIRelationship != AIRelationship.Hostile) return;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);
            foreach (var hit in hits)
            {

                AIControler ai = hit.transform.GetComponent<AIControler>();
                if (ai != null && ai != this)
                {
                    ai.Aggrevate();
                }
            }

        }

        private bool IsAggrevated()
        {
            if (timeSinceAggrevated < aggrevationCoolDownTime)
            {
                //aggrevated
                return true;
            }
            return DistanceToCombatTarget() <= chaseDistance;
        }

        private float DistanceToCombatTarget()
        {
            float shortestDistancetToTarget = Mathf.Infinity;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var player in players)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < shortestDistancetToTarget)
                {
                    shortestDistancetToTarget = distance;
                    combatTargetGameObject = player;
                }
            }
            return shortestDistancetToTarget;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

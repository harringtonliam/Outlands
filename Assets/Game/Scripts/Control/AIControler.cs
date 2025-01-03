using RPG.Attributes;
using RPG.Buildings;
using RPG.Combat;
using RPG.Core;
using RPG.FurnitureControl;
using RPG.Movement;
using System;
using UnityEngine;

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
        [SerializeField] HouseSettings homeHouse;

        Mover mover;
        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        float timeAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        FurnitureController furnitureController;

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
            furnitureController = GetComponent<FurnitureController>();

            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

            if (GetComponent<Health>().IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithSuspicsion()) return;
            if (InteractWithPatrolPath()) return;
            if (InteractWithDestination()) return;
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
            float distanceToPosition = Vector3.Distance(transform.position, positionToCheck);
            if (distanceToPosition <= waypointTolerance)
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

        
        private bool InteractWithSuspicsion()
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

            if (InteractWithFurniture(destination))
            {
                return true;
            }

            if (AtPosition(destination.transform.position))
            {
                //ActionScheduler actionSchduler = GetComponent<ActionScheduler>();
                //actionSchduler.CancelCurrentAction();
            }
            else
            {
                GetOffFurnitureIfNeeded();
                mover.StartMovementAction(destination.transform.position, patrolSpeedFraction);
            }

            return true;
        }

        private bool InteractWithHomeHouse()
        {
            if (homeHouse == null) return false;
            var positionToGoTo = homeHouse.DayTimeDestinations[0];
            if (homeHouse.IsNightTime())
            {
                positionToGoTo = homeHouse.NightTimeDestinations[0];
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

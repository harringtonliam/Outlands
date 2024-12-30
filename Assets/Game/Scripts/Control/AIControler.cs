using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;
using RPG.Buildings;

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
            if(AtPosition(destination.transform.position))
            {
                //ActionScheduler actionSchduler = GetComponent<ActionScheduler>();
                //actionSchduler.CancelCurrentAction();
            }
            else
            {
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

            if(AtPosition(positionToGoTo.transform.position))
            {
                //ActionScheduler actionSchduler = GetComponent<ActionScheduler>();
                //actionSchduler.CancelCurrentAction();
            }
            else
            {
                mover.StartMovementAction(positionToGoTo.transform.position, patrolSpeedFraction);
            }
            return true;
        }

        private bool InteractWithGuardPosition()
        {
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

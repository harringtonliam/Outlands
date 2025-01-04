using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.GameTime;
using System;
using System.Linq;

namespace RPG.Control
{
    public class AIBehaviour : MonoBehaviour
    {
        [SerializeField] BehaviourDescription[] behaviourDescriptions;

        [System.Serializable]
        public class BehaviourDescription
        {
            public bool appliesToAllMonths = true;
            public int month;
            public bool appliesToSpecificWeekDay = false;
            public int weekDay;
            public bool appliesToAllDays = true;
            public int dayFrom;
            public int dayTo;
            public int hourFrom;
            public int hourTo;
            public PatrolPath patrolPath;
            public bool doPatrolPathOnce = false;
            public float waypointPauseTime = 2f;
            public float patrolSpeedFraction = 0.2f;
            public Transform transformDestination;

            public string Print()
            {
                return "Behaviour Description applies to all months " + appliesToAllMonths + " month  " + month + " appliestospecfic weekday " + appliesToSpecificWeekDay + ": " + weekDay +"appplies to all days " + appliesToAllDays + " day from and to " + dayFrom + " " + dayTo + " hours from and to " + hourFrom + " " + hourTo;
            }
        }

        AIControler aIControler;
        GameTimeContoller gameTimeContoller;

        // Start is called before the first frame update
        void Start()
        {
            aIControler = GetComponent<AIControler>();
            gameTimeContoller = FindFirstObjectByType<GameTimeContoller>();
            gameTimeContoller.hourHasPassed += CheckBehaviour;
        }


        private void CheckBehaviour()
        {
            if (behaviourDescriptions.Length <= 0) return;

            var sortedBehaviours = behaviourDescriptions.OrderBy(m => m.appliesToAllMonths).ThenByDescending(w => w.appliesToSpecificWeekDay).ThenBy(d => d.appliesToAllDays).ToArray();
            for (int i = 0; i < sortedBehaviours.Length; i++)
            {
                //Debug.Log("Checking Behaviour sorted array " + i + " " + sortedBehaviours[i].Print()); 
                if (BehaviourApplies(sortedBehaviours[i]))
                {
                    ApplyBehaviour(sortedBehaviours[i]);
                    return;
                }
            }
            ApplyNoBehaviour();
        }

        private void ApplyBehaviour(BehaviourDescription behaviourDescription)
        {
            aIControler.SetPatrolPath(behaviourDescription.patrolPath);
            aIControler.SetDesitination(behaviourDescription.transformDestination.gameObject);
        }

        private void ApplyNoBehaviour()
        {
            aIControler.SetPatrolPath(null);
            aIControler.SetDesitination(null);
        }

        private bool BehaviourApplies(BehaviourDescription behaviourDescription)
        {
            bool useThisBehavior = true;

            if(!behaviourDescription.appliesToAllMonths && behaviourDescription.month != gameTimeContoller.CurrentLocalMonth)
            {
                useThisBehavior = false;
            }
            if (behaviourDescription.appliesToSpecificWeekDay && behaviourDescription.weekDay != gameTimeContoller.CurrentLocalDayOfWeek)
            {
                //Debug.Log("Failed weekday check " + behaviourDescription.appliesToSpecificWeekDay + " " + behaviourDescription.weekDay + " " + gameTimeContoller.GetCurrentDayOfWeek());
                useThisBehavior = false;
            }
            if (!behaviourDescription.appliesToAllDays && (gameTimeContoller.CurrentLocalDayOfMonth < behaviourDescription.dayFrom || gameTimeContoller.CurrentLocalDayOfMonth > behaviourDescription.dayTo))
            {
                //Debug.Log("Failed day check " + behaviourDescription.appliesToAllDays + " " + behaviourDescription.dayFrom + " " + behaviourDescription.dayTo + " " + gameTimeContoller.CurrentDayOfMonth);
                useThisBehavior = false;
            }
            if (gameTimeContoller.CurrentLocalHour < behaviourDescription.hourFrom  || gameTimeContoller.CurrentLocalHour > behaviourDescription.hourTo)
            {
                //Debug.Log("Failed hour check "  + " " + behaviourDescription.hourFrom + " " + behaviourDescription.hourTo + " " + gameTimeContoller.CurrentHour);
                useThisBehavior = false;
            }

            return useThisBehavior;
        }
    }
}


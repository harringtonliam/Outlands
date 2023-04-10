using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "StrengthScoreTable", menuName = "Stats/Make New Strength Score Table", order = 30)]
    public class StrengthScoreBonus : ScriptableObject
    {
        [SerializeField] StrengthScoreDamage[] strengthScoreDamages;


        public float GetDamage(int strength)
        {
            float strengthDamage = 0f;
            for (int i = 0; i < strengthScoreDamages.Length; i++)
            {
                if (strength > strengthScoreDamages[i].strengthScore)
                {
                    strengthDamage = strengthScoreDamages[i].pointsOfDamage;
                }
            }

            return strengthDamage;
        }


    }

    [System.Serializable]
    public class StrengthScoreDamage
    {
        public int strengthScore;
        public float pointsOfDamage;
    }

}



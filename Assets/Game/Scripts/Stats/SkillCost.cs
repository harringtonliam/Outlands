using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "SkillCost", menuName = "Stats/New Skill Cost", order = 20)]
    public class SkillCost : ScriptableObject
    {
        [SerializeField] SkillCostPerLevel[] skillCostPerLevels;

        public int GetCostPerLevel(PrimarySkillArea characterPrimarySkillArea, PrimarySkillArea primarySkillArea,  int level)
        {

            SkillCostPerLevel skillCostPerLevel = null;

            skillCostPerLevel = FindMatchingPrimarySkillArea(primarySkillArea, skillCostPerLevel);

            if (skillCostPerLevel == null) return -1;

            int calculatedSkillCost = 0;

            if (level <= skillCostPerLevel.costPerLevelPrimaries.Length)
            {
                calculatedSkillCost = skillCostPerLevel.costPerLevelPrimaries[level];
            }
            else
            {
                return -1;
            }

            if (primarySkillArea != characterPrimarySkillArea)
            {
                calculatedSkillCost = skillCostPerLevel.nonPrimaryMultiplier * calculatedSkillCost;
            }

            return calculatedSkillCost;
        }

        private SkillCostPerLevel FindMatchingPrimarySkillArea(PrimarySkillArea primarySkillArea, SkillCostPerLevel skillCostPerLevel)
        {
            for (int i = 0; i < skillCostPerLevels.Length; i++)
            {
                if (skillCostPerLevels[i].primarySkillArea == primarySkillArea)
                {
                    skillCostPerLevel = skillCostPerLevels[i];
                    break;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class SkillCostPerLevel
    {
        public PrimarySkillArea primarySkillArea;
        public int nonPrimaryMultiplier;
        public int[] costPerLevelPrimaries;
    }

}

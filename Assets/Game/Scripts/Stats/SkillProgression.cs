using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{

    [CreateAssetMenu(fileName = "SkillProgression", menuName = "Stats/New Skill Progession", order = 10)]
    public class SkillProgression : ScriptableObject
    {
        [SerializeField]
        SkillDescription[] skillDescriptions;


        public float GetSkillChance(PrimarySkillArea primarySkillArea, Skill skill, int level)
        {
            float skillChance = 0f;

            SkillDescription skillDescription = GetSkillDescription(primarySkillArea, skill);
            if (skillDescription == null) return skillChance;

            skillChance = skillDescription.baseSuccessRate + (skillDescription.successPerSkillLevel * level);

            return skillChance;

        }

        public float GetSkillChance(Skill skill, int level)
        {
            float skillChance = 0f;

            SkillDescription skillDescription = GetSkillDescription(skill);
            if (skillDescription == null) return skillChance;

            skillChance = skillDescription.baseSuccessRate + (skillDescription.successPerSkillLevel * level);

            return skillChance;

        }

        public SkillDescription GetSkillDescription(PrimarySkillArea primarySkillArea, Skill skill)
        {
            for (int i = 0; i < skillDescriptions.Length; i++)
            {
                if (skillDescriptions[i].primarySkillArea == primarySkillArea && skillDescriptions[i].skill == skill)
                {
                    return skillDescriptions[i];
                }  
            }

            return null;
        }

        public SkillDescription GetSkillDescription(Skill skill)
        {
            for (int i = 0; i < skillDescriptions.Length; i++)
            {
                if (skillDescriptions[i].skill == skill)
                {
                    return skillDescriptions[i];
                }
            }

            return null;
        }

    }



    [System.Serializable]
    public class SkillDescription
    {
        public PrimarySkillArea primarySkillArea;
        public Skill skill;
        public float baseSuccessRate;
        public float successPerSkillLevel;
    }


}

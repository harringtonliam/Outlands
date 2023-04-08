using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class CharacterSkillRecord : MonoBehaviour, ISaveable
    {
        [SerializeField] SkillProgression skillProgression;
        [SerializeField] SkillCost skillCost;
        [SerializeField] PrimarySkillArea characterPrimarySkillArea;
        [SerializeField] CharacterSkillLevel[] characterSkillLevels;

        public int GetSkillLevel(Skill skill)
        {
            for (int i = 0; i < characterSkillLevels.Length; i++)
            {
                if (skill == characterSkillLevels[i].skill)
                {
                    return characterSkillLevels[i].skillLevel;
                }
            }

            return 0;
        }

        public float GetSkillChance(Skill skill)
        {
            int skillLevel = GetSkillLevel(skill);
            return skillProgression.GetSkillChance(skill, skillLevel);
        }

        public object CaptureState()
        {
            return characterSkillLevels;
        }

        public void RestoreState(object state)
        {
            var restoredCharaceterSkillLevels = (CharacterSkillLevel[])state;

            characterSkillLevels = new CharacterSkillLevel[restoredCharaceterSkillLevels.Length];
            for (int i = 0; i < restoredCharaceterSkillLevels.Length; i++)
            {
                characterSkillLevels[i].skill = restoredCharaceterSkillLevels[i].skill;
                characterSkillLevels[i].skillLevel=   restoredCharaceterSkillLevels[i].skillLevel;
            }

        }

    }

    [System.Serializable]
    public class CharacterSkillLevel
    {
        [SerializeField] public Skill skill;
        [SerializeField] public int skillLevel;
    }

}



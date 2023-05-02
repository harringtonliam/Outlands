using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using UnityEngine.Events;
using System;
using RPG.InventoryControl;

namespace RPG.Attributes
{

    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] UnityEvent<float> takeDamage;
        [SerializeField] UnityEvent onDie;

        public event Action healthUpdated;
        public event Action deathUpdated;

        CharacterSheet characterSheet;
        string characterName;

        EquipedArmourHandler equipedArmourHandler;


        float currentStamina = -1f;

        bool isDead = false;

        public bool IsDead { get { return isDead; } }

        public float HealthPoints {
            get { return currentStamina; }
        }

        void Start()
        {
            characterSheet = GetComponent<CharacterSheet>();
            if (characterSheet != null)
            {
                characterName = characterSheet.CharacterName;
            }
            if (currentStamina < 0)
            {
                currentStamina = GetMaxStamina();
            }
            if (healthUpdated != null)
            {
                healthUpdated();
            }

            equipedArmourHandler = GetComponent<EquipedArmourHandler>();


        }

        private void OnEnable()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            if (baseStats != null)
            {
                baseStats.onLevelUp += BaseStats_onLevelUp;
            }
        }

        private void OnDisable()
        {
            BaseStats baseStats = GetComponent<BaseStats>();
            if (baseStats != null)
            {
                baseStats.onLevelUp -= BaseStats_onLevelUp;
            }
        }

        public float GetPercentage()
        {
            return (currentStamina / GetMaxStamina()) * 100;
        }

        public float GetMaxStamina()
        {
            CharacterAbilities characterAbilities = GetComponent<CharacterAbilities>();
            float maxStamina = characterAbilities.GetAbilityScore(Ability.Stamina);
            return maxStamina;
        }

        public void TakeDamage(float damage, GameObject instigator)
        {
            TakeDamage(damage, instigator, ArmourType.Inertia);
        }

        public void TakeDamage(float damage, GameObject instigator, ArmourType defense)
        {
            float damageReducedBy = equipedArmourHandler.GetDamageReductionAmount(defense, damage);
            float calculatedDamage = (int)(Math.Ceiling(Mathf.Max(damage - damageReducedBy, 0f)));
            string logMessage = "Take damage Amour Type =  {0} , damage= {1} ,  damageReducedBy =  {2}, finalDamage = {3} ";

            Debug.Log(String.Format(logMessage, defense.ToString(), damage,   damageReducedBy, calculatedDamage));

            currentStamina = Mathf.Max(currentStamina - calculatedDamage, 0);
            if (damage > 0)
            {
                WriteDamageToConsole(calculatedDamage);
            }
            if (healthUpdated != null)
            {
                healthUpdated();
            }

            if (currentStamina <= 0)
            {
                AwardExperience();
                Die();
            }
            else if (instigator.tag == "Player")
            {
                takeDamage.Invoke(damage);
            }

            
            

        }



        public void Heal(float healing)
        {
            currentStamina = Mathf.Min(currentStamina + healing, GetMaxStamina());
            if (healthUpdated != null)
            {
                healthUpdated();
            }
        }

        private void AwardExperience()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Experience experience = player.GetComponent<Experience>();
            if (experience != null && !isDead)
            {
                float experienceGained = gameObject.GetComponent<BaseStats>().GetStat(Stat.ExperienceReward);
                experience.GainExperience(experienceGained);
            }
        }



        public void Die()
        {
            if (isDead) return;
            onDie.Invoke();
            if (deathUpdated != null)
            {
                deathUpdated();
            }

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("die");
            }

            ResizeCapsuleColliderOnDeath();

            isDead = true;
            WriteDeathToConsole();
            ActionScheduler actionScheduler = GetComponent<ActionScheduler>();
            if (actionScheduler != null)
            {
                actionScheduler.CancelCurrentAction();
            }
        }

        private void ResizeCapsuleColliderOnDeath()
        {
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                capsuleCollider.height = capsuleCollider.height / 10f;
                capsuleCollider.center = capsuleCollider.center / 4f;
            }
        }

        private void WriteDamageToConsole(float damage)
        {
                GameConsole.AddNewLine(characterName + " takes " + damage.ToString() + " damage.");
        }



        private void WriteDeathToConsole()
        {
            GameConsole.AddNewLine(characterName + " Death!");
        }

        private void BaseStats_onLevelUp()
        {
            float newHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            currentStamina = newHealthPoints;
            if (healthUpdated != null)
            {
                healthUpdated();
            }
        }

        public object CaptureState()
        {
            return currentStamina;
        }

        public void RestoreState(object state)
        {
            currentStamina = (float)state;
            if (currentStamina <= 0)
            {
                onDie.Invoke();
                Die();
            }
        }
    }
}

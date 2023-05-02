using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.Stats;
using System.Text;

namespace RPG.Combat
{
    public class Fighting : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeaponConfig = null;
        [SerializeField] StrengthScoreBonus strengthScoreBonus;
        [SerializeField] Health target;

        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        Weapon currentWeapon;
        WeaponStore weaponStore;
        AmmunitionStore ammoStore;
        CharacterSheet characterSheet;
        CharacterSkillRecord characterSkillRecord;
        CharacterAbilities characterAbilities;


        private void Awake()
        {
            weaponStore = GetComponent<WeaponStore>();

            if (weaponStore)
            {
                weaponStore.storeUpdated += UpdateWeapon;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (currentWeaponConfig == null)
            {
               currentWeapon = EquipWeapon(defaultWeaponConfig);
            }

            ammoStore = GetComponent<AmmunitionStore>();
            characterSheet = GetComponent<CharacterSheet>();
            characterSkillRecord = GetComponent<CharacterSkillRecord>();
            characterAbilities = GetComponent<CharacterAbilities>();
        }



        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            Mover mover = GetComponent<Mover>();

            if (target != null)
            {
                if (target.IsDead)
                {
                    return;
                }

                if(mover != null) mover.MoveTo(target.transform.position, 1f);
                if (GetIsInRange(target.transform))
                {
                    if(mover != null) mover.Cancel();
                   
                    AttackBehaviour();
                }
            }
        }

        public Health GetTarget()
        {
            return target;
        }


        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Animator animator = GetComponent<Animator>();
            currentWeapon = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
            return currentWeapon;
        }

        private void UpdateWeapon()
        {
            var weaponConfig = weaponStore.GetActiveWeapon() as WeaponConfig;
            if (weaponConfig != null)
            {
                EquipWeapon(weaponConfig);
            }
            else
            {
                EquipWeapon(defaultWeaponConfig);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Events
        public void Hit()
        {
            if (target == null) return;

            if (!UseAmmunition())
            {
                ChangeWeapon();
                return;
            }
            
            if (currentWeapon != null)
            {
                currentWeapon.OnHit();
            }

          
            if (currentWeaponConfig.IsRangedWeapon && !CheckLineOfSight())
            {
                WriteToConsole("Line of sight blocked for ranged weapon.");
                return;
            }

            float calculatedDamage = 0;

            if (AttackRollSuccessful())
            {
                calculatedDamage = GetWeaponDamage(out string breakdown);
            }
            else
            {
                return;
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, calculatedDamage);
            }
            else
            { 
                target.TakeDamage(calculatedDamage, gameObject, currentWeaponConfig.Defense);
            }
        }

        private float AddPunchingScoreDamage()
        {
            if (!currentWeaponConfig.AddPunchingDamage) return 0f;
            if (strengthScoreBonus == null) return 0f;
    
            float punchingScoreDamgae = strengthScoreBonus.GetDamage(characterAbilities.GetAbilityScore(Ability.Strength));

            return punchingScoreDamgae;
        }

        public int GetWeaponDamage(out string breakdown)
        {
            int weaponDamage = currentWeaponConfig.CalcWeaponDamage();
            int punchingScoreDamage = (int)AddPunchingScoreDamage();

            StringBuilder breakdownBuilder = new StringBuilder();

            breakdownBuilder.Append(currentWeaponConfig.WeaponDamageBreakdown());
            if (punchingScoreDamage>0)
            {
                breakdownBuilder.Append("\r\n");
                breakdownBuilder.Append(Ability.Strength.ToString()); 
                breakdownBuilder.Append(" bonus: ");
                breakdownBuilder.Append(punchingScoreDamage.ToString());
            }

            breakdown = breakdownBuilder.ToString();

            return weaponDamage + punchingScoreDamage;
        }

        private void ChangeWeapon()
        {
            //find a different weapon
            weaponStore.FindWeaponToChangeTo();
        }

        private bool UseAmmunition()
        {
            if (currentWeaponConfig.AmmunitionType == AmmunitionType.None)
            {
                return true;
            }

            AmmunitionStore ammoStore = GetComponent<AmmunitionStore>();

            //search for ammo type in ammostoreslot
            int ammoStoreSlot = ammoStore.FindAmmunitionType(currentWeaponConfig.AmmunitionType);

            //if none found return
            if (ammoStoreSlot < 0)
            {
                WriteToConsole("Out Of Ammunition");
                return false;
            }

            if (ammoStore.GetNumberOfUses(ammoStoreSlot) < 1)
            {
                WriteToConsole("Out Of Ammunition");
                return false;
            }

            ammoStore.UseAmmunition(ammoStoreSlot, 1);
            return true;
        }

        private bool CheckLineOfSight()
        {
            if (target == null) return false;

            Vector3 lineOfSightStart = currentWeaponConfig.GetTransform(rightHandTransform, leftHandTransform).position;
            lineOfSightStart.x = lineOfSightStart.x + 0.15f;
            Vector3 lineOfSightEnd = GetAimLocation();

            Vector3 lineOfSiteDirection = (lineOfSightEnd - lineOfSightStart).normalized;
    
            Ray ray = new Ray(lineOfSightStart, lineOfSiteDirection);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, currentWeaponConfig.WeaponRange);
            Debug.Log("Check line of site for " + gameObject.name + " target=" + target.gameObject.name +" hit=" + hit.transform.gameObject.name);
            Health targetHealth = hit.transform.GetComponent<Health>();

            if (targetHealth == null || targetHealth != target)
            {
                return false;
            }
           
            return true;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

 

        private bool AttackRollSuccessful()
        {
            string breakdown = string.Empty;
            int chanceToHit = CalculateChanceToHit(out breakdown);
            int attackRoll = Dice.RollDice(100, 1);
            if (attackRoll >= 99) return false;
            if (attackRoll <= 1) return true;
            if (attackRoll > chanceToHit)
            {
                WriteToConsole("attack missed");
            }

            return attackRoll <= chanceToHit;
        }

        private int CalculateChanceToHit(out string braakdown)
        {
            int chanceToHit = 0;
            string localbreakdown = string.Empty;
            int strenght = characterAbilities.GetAbilityScore(Ability.Strength);
            int dexterity = characterAbilities.GetAbilityScore(Ability.Dexterity);
            if (currentWeaponConfig.IsRangedWeapon)
            {
                chanceToHit = CalculateRangedWeaponChanceToHit(dexterity);
            }
            else
            {
               chanceToHit = CalcMeleeChanceToHit (strenght, dexterity, out localbreakdown);
            }

            braakdown = localbreakdown;

            return chanceToHit;
        }

        public int CalculateBaseChanceToHit(out string breaddown)
        {
            int chanceToHit = 0;
            string localbreakdown = string.Empty;
            int strenght = characterAbilities.GetAbilityScore(Ability.Strength);
            int dexterity = characterAbilities.GetAbilityScore(Ability.Dexterity);
            if (currentWeaponConfig.IsRangedWeapon)
            {
                chanceToHit = CalculateRangedWeaponChanceToHit(dexterity);
            }
            else
            {
                chanceToHit = CalculateBaseMeleeChanceToHit(strenght, dexterity, out localbreakdown);
            }

            breaddown = localbreakdown;
            return chanceToHit;
        }

        private int CalcMeleeChanceToHit(int strenght, int dexterity, out string breakdown)
        {
            int chanceToHit = CalculateBaseMeleeChanceToHit(strenght, dexterity, out breakdown);
            
            if(IsAttackFromBehind()  || IsTargetStunned())
            {
                chanceToHit += 20;
            }
            if(IsEncumbered(target))
            {
                chanceToHit += 20;
            }

            //Todo: subtract 15 is target is defending itself


            return chanceToHit;
        }

        private int CalculateBaseMeleeChanceToHit(int strenght, int dexterity, out string breakdown)
        {
            breakdown = string.Empty;
            int chanceToHit = 0;
            Health attackerHealth = GetComponent<Health>();

            int statUsed = 0;
            string statUsedText = string.Empty;
            if (strenght > dexterity)
            {
                statUsed = strenght;
                statUsedText = Ability.Strength.ToString();
            }
            else
            {
                statUsed = dexterity;
                statUsedText = Ability.Dexterity.ToString();
            }
                
            
            int baseChanceToHit = DivideByTwoRoundedUp(statUsed);
            int skillBonus = GetWeaponSkillBonus(currentWeaponConfig.WeaponSkill);
            int weaponBonus = currentWeaponConfig.WeaponToHitBonus;
            int encumberedPenalty = 0;
            if (IsEncumbered(attackerHealth))
            {
                encumberedPenalty -= 10;
            }
            int injuredPenalty = 0;
            if (attackerHealth.HealthPoints <= (attackerHealth.GetMaxStamina() / 2))
            {
                injuredPenalty -= 10;
            }

            chanceToHit = baseChanceToHit + skillBonus + weaponBonus + encumberedPenalty + injuredPenalty;
            StringBuilder breakdownbuilder = new StringBuilder();
            breakdownbuilder.Append("Base chance to hit: " + baseChanceToHit + "% (" + statUsedText + ")\r\n");
            if (skillBonus > 0)
            {
                breakdownbuilder.Append("Skill Bonus " + currentWeaponConfig.WeaponSkill + ": " + skillBonus + "%\r\n");
            }
            if (weaponBonus > 0)
            {
                breakdownbuilder.Append("Weapon Bonus " + currentWeaponConfig.DisplayName + ": " + weaponBonus + "%\r\n");
            }
            if (encumberedPenalty < 0)
            {
                breakdownbuilder.Append("Encumbered Penalty: " + encumberedPenalty + "%\r\n");
            }
            if (injuredPenalty < 0)
            {
                breakdownbuilder.Append("Injured Penalty: " + injuredPenalty + "%\r\n");
            }
            breakdownbuilder.Append("Total: " + chanceToHit + "%");

            breakdown = breakdownbuilder.ToString();

            return chanceToHit;
        }

        private int CalculateRangedWeaponChanceToHit(int dexterity)
        {
            int chanceToHit = 0;
            chanceToHit = DivideByTwoRoundedUp(dexterity);
            //TODO add all other modifiers
            return chanceToHit;
        }

        private int DivideByTwoRoundedUp(int statUsed)
        {
            return (statUsed / 2) + (statUsed % 2 > 0 ? 1 : 0);
        }

        private int GetStatModifier()
        {
            CharacterAbilities characterAbilities = GetComponent<CharacterAbilities>();
            if (characterAbilities == null) return 0;

            return 0;

        }

        public void Shoot()
        {
            Hit();
        }
        //End of animation events

        private bool GetIsInRange(Transform targetTransform)
        {
            return currentWeaponConfig.WeaponRange >= Vector3.Distance(targetTransform.position, transform.position);
        }

        private bool IsAttackFromBehind()
        {
            float angle = Vector3.Angle(target.transform.forward, transform.forward);
            if (angle <= 15f)
            {
                Debug.Log("Attack from behind");
                return true;
            }

            return false;
        }

        private int GetWeaponSkillBonus(Skill skill)
        {
            return (int)characterSkillRecord.GetSkillChance(skill);
        }

        //TODO: check if target is stunned
        private bool IsTargetStunned()
        {
            return false;
        }

        public bool IsEncumbered(Health target)
        {
            Encumberance encumberance = target.GetComponent<Encumberance>();
            if (encumberance == null) return false;
            return encumberance.IsEncumbered();
        }

        private void WriteToConsole(string consoleText)
        {
            string characterName = string.Empty;
            if (characterSheet != null)
            {
                characterName = characterSheet.CharacterName;
            }
            GameConsole.AddNewLine(characterName + ": " + consoleText);
        }

        public void Attack(GameObject combatTarget)
        {

            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>(); ;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;


            if (GetComponent<Mover>() != null)
            {
                if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)
                        && !GetIsInRange(combatTarget.transform))
                {
                    return false;
                }
            }

            if (GetComponent<Mover>() == null)
            {
                if (!GetIsInRange(combatTarget.transform))
                {
                    return false;
                }
            }



            Health targetHealth = combatTarget.GetComponent<Health>();
            if (targetHealth.IsDead)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            if (GetComponent<Mover>() != null)
            {
                GetComponent<Mover>().Cancel();
            }
           
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            if (currentWeaponConfig!= null)
            {
                return currentWeaponConfig.name;
            }
            else
            {
                return null;
            }
    
        }

        public void RestoreState(object state)
        {
            if (state != null)
            {
                string weaponName = (string)state;
                WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
                EquipWeapon(weapon);
            }

        }

    }
}

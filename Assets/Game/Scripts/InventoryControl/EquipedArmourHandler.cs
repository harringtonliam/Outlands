using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.InventoryControl
{
    public class EquipedArmourHandler : MonoBehaviour
    {
        Equipment equipment;

        // Start is called before the first frame update
        void Start()
        {
            equipment = GetComponent<Equipment>();
        }

        public float GetPercentDamageReduction(ArmourType armourType)
        {
            float percentDamageReduction = 0f;

            Armour armourToUse = null;
            EquipLocation slotToUse;

            foreach (var slot in equipment.GetAllPopulatedSlots())
            {
                var equipedItem = equipment.GetItemInSlot(slot);
                var equipedArmour = equipedItem as Armour;
                if (equipedArmour != null && equipedArmour.ArmourType == armourType && equipedArmour.PercentDamageAbsorbtion >= percentDamageReduction)
                {
                    armourToUse = equipedArmour;
                    percentDamageReduction = equipedArmour.PercentDamageAbsorbtion;
                    slotToUse = slot;
                }
            }



            return percentDamageReduction;

        }

        public float GetDamageReductionAmount (ArmourType armourType, float damage)
        {
            float damageReducedBy = 0f;
            float percentDamageReduction = 0f;
            Armour armourToUse = null;
            EquipLocation slotToUse = EquipLocation.Body;

            foreach (var slot in equipment.GetAllPopulatedSlots())
            {
                var equipedItem = equipment.GetItemInSlot(slot);
                var equipedArmour = equipedItem as Armour;
                if (equipedArmour != null && equipedArmour.ArmourType == armourType && equipedArmour.PercentDamageAbsorbtion >= percentDamageReduction)
                {
                    armourToUse = equipedArmour;
                    percentDamageReduction = equipedArmour.PercentDamageAbsorbtion;
                    slotToUse = slot;
                }
            }

            if (armourToUse != null)
            {
                damageReducedBy =  Mathf.Min((int)(damage * (percentDamageReduction / 100f)), equipment.GetNumberOfUsesinSlot(slotToUse));
                equipment.UpdateRemainingUses(slotToUse, (int)damageReducedBy * -1);
            }

            return damageReducedBy;
        }
    }



}



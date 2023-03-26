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

            Armour armourToUse = null; ;

            foreach (var slot in equipment.GetAllPopulatedSlots())
            {
                var equipedItem = equipment.GetItemInSlot(slot);
                var equipedArmour = equipedItem as Armour;
                if (equipedArmour != null && equipedArmour.ArmourType == armourType && equipedArmour.PercentDamageAbsorbtion >= percentDamageReduction)
                {
                    armourToUse = equipedArmour;
                    percentDamageReduction = equipedArmour.PercentDamageAbsorbtion;
                }
            }

            return percentDamageReduction;

        }
    }



}



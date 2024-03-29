using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;


namespace RPG.InventoryControl
{ 
    [CreateAssetMenu(fileName = "Armour", menuName = "Items/Make New Armour", order = 3)]
    public class Armour : EquipableItem
    {
        [SerializeField] ArmourType armourType;
        [SerializeField] float percentDamageAbsorbtion = 50f;

        public float PercentDamageAbsorbtion
        {
            get { return percentDamageAbsorbtion; }
        }

        public ArmourType ArmourType
        {
            get { return armourType; }
        }

    }
}

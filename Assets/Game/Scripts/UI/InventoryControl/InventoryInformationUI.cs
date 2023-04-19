using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using RPG.Combat;
using RPG.Control;
using RPG.Stats;
using RPG.InventoryControl;
using System;

namespace RPG.UI.InventoryControl
{
    public class InventoryInformationUI : SelectedPlayerBasedUI
    {
        [SerializeField] TextMeshProUGUI inventoryInfo;

        WeaponStore weaponStore;
        Encumberance encumberance;
        EquipedArmourHandler equipedArmourHandler;

        void OnEnable()
        {
            weaponStore = PlayerSelector.GetFirstSelectedPlayer().GetComponent<WeaponStore>();
            encumberance = PlayerSelector.GetFirstSelectedPlayer().GetComponent<Encumberance>();
            equipedArmourHandler = PlayerSelector.GetFirstSelectedPlayer().GetComponent<EquipedArmourHandler>(); ;

            weaponStore.storeUpdated += Redraw;
            encumberance.encumberanceUpdated += Redraw;
            Redraw();
        }

        private void OnDisable()
        {
            if (weaponStore != null)
            {
                weaponStore.storeUpdated -= Redraw;
            }
            if (encumberance != null)
            {
                encumberance.encumberanceUpdated -= Redraw;
            }
        }

        public void Redraw()
        {
            var player = PlayerSelector.GetFirstSelectedPlayer();
            Fighting fighting = player.GetComponent<Fighting>();

            StringBuilder infoBuilder = new StringBuilder(); 

            string chanceToHitInfoText = string.Empty;
            int chanceToHit = fighting.CalculateBaseChanceToHit(out chanceToHitInfoText);
            infoBuilder.Append("Chance To Hit\r\n");
            infoBuilder.Append(chanceToHitInfoText);


            string weaponDamageInfoText = string.Empty;
            int weaponDamage = fighting.GetWeaponDamage(out weaponDamageInfoText);
            infoBuilder.Append("\r\n");
            infoBuilder.Append("\r\nWeapon Damage\r\n");
            infoBuilder.Append(weaponDamageInfoText);

            infoBuilder.Append("\r\n");
            infoBuilder.Append("\r\nDefense\r\n");
            infoBuilder.Append(DefenceInfo());

            infoBuilder.Append("\r\n");
            infoBuilder.Append("\r\nCarrying Capacity\r\n");
            infoBuilder.Append(EncumberanceInfo());

            inventoryInfo.text = infoBuilder.ToString();
        }

        public override void SelectedPlayerChanged()
        {
            OnDisable();
            OnEnable();
        }

        private string EncumberanceInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Max unencumbered capacity: ");
            builder.Append(encumberance.GetMaxUnencumberedCapacity().ToString());
            builder.Append("kg \r\n");

            builder.Append("Currently carrying: ");
            builder.Append(encumberance.GetTotalCarried().ToString());
            builder.Append("kg \r\n");

            if (encumberance.IsEncumbered())
            {
                builder.Append("*Encumbered**");
            }

            return builder.ToString();

        }

        private string DefenceInfo()
        {
            StringBuilder armourInfo = new StringBuilder();

            var values = Enum.GetValues(typeof(ArmourType));
            foreach (ArmourType item in values)
            {
                float damageReduction = equipedArmourHandler.GetPercentDamageReduction(item);
                if (damageReduction > 0)
                {
                    armourInfo.Append(item.ToString());
                    armourInfo.Append(": ");
                    armourInfo.Append(damageReduction.ToString());
                    armourInfo.Append("% damage reduction");
                    armourInfo.Append("\r\n");
                }
            }
            if (armourInfo.ToString() == string.Empty)
            {
                armourInfo.Append("*None*\r\n");
            }

            return armourInfo.ToString();
        }
    }
}






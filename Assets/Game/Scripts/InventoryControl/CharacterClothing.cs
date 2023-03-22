using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.InventoryControl
{
    public class CharacterClothing : MonoBehaviour
    {
        [SerializeField]
        ClothingConfig[] clothingConfigs;


        [Serializable]
        public struct ClothingConfig
        {
            public EquipLocation equipLocation;
            public InventoryItem inventoryItem;
            public Transform characterMesh;

        }

        Equipment equipment;

        // Start is called before the first frame update
        void Start()
        {
            equipment = GetComponent<Equipment>();
            equipment.equipmentUpdated += ProcesssEquipmenyUpdated;
            ProcesssEquipmenyUpdated();
        }


        void ProcesssEquipmenyUpdated()
        {
            SetClothing(EquipLocation.Helmet);
            SetClothing(EquipLocation.Body);
        }

        void SetClothing(EquipLocation equipLocation)
        {
            InventoryItem itemEquiped = equipment.GetItemInSlot(equipLocation);
            for (int i = 0; i < clothingConfigs.Length; i++)
            {
                if (clothingConfigs[i].equipLocation == equipLocation)
                {
                    if (itemEquiped == clothingConfigs[i].inventoryItem)
                    {
                        clothingConfigs[i].characterMesh.gameObject.SetActive(true);
                    }
                    else
                    {
                        clothingConfigs[i].characterMesh.gameObject.SetActive(false);
                    }
                }
            }

        }


    }
}



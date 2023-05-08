using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Attributes
{
    public class CharacterSheet : MonoBehaviour
    {
        [SerializeField] string characterName = "No Name";
        [SerializeField] string rank = "No Rank";
        [SerializeField] Sprite portrait = null;
        [SerializeField] bool isMainPlayerCharacter = false;


        public string CharacterName { get { return characterName; } }
        public Sprite Portrait { get { return portrait; } }

        public string Rank { get { return rank; } }

        public bool IsMainPlayerCharacter { get { return isMainPlayerCharacter; } }

    }
}



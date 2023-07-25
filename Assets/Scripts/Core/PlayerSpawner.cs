using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] List<GameObject> charactersList = new List<GameObject>();
        GameObject character;

        public GameObject GetCharacter()
        {
            return character;
        }

        public void SetCharacter(CharacterClass characterClass)
        {
            foreach (var character in charactersList)
            {
                if (character.GetComponent<BaseStats>().GetCharacterClass() == characterClass)
                {
                    this.character = character;
                    Debug.Log("Character assigned! " + characterClass);
                    return;
                }
            }

        }
    }
}
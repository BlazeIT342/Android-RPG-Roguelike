using RPG.Abilities;
using RPG.Combat;
using RPG.Perks;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Characters Data", menuName = "RPG Time Killer/Stats/New Characters Data", order = 0)]
    public class CharactersData : ScriptableObject
    {
        [SerializeField] Character[] characters;
        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookup();

            if (!lookupTable[characterClass].ContainsKey(stat))
            {
                return 0;
            }

            float[] levels = lookupTable[characterClass][stat];

            if (levels.Length == 0)
            {
                return 0;
            }
            if (levels.Length < level)
            {
                return levels[levels.Length - 1];
            }

            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();
            float[] levels = lookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;
            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (Character progressionClass in characters)
            {
                var startLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.BaseStats)
                {
                    startLookupTable[progressionStat.stat] = progressionStat.levels;
                }

                lookupTable[progressionClass.characterClass] = startLookupTable;
            }
        }

        public IEnumerable<Character> GetCharacters()
        {
            foreach (Character character in characters)
            {
                yield return character;
            }                    
        }

        public bool GetIsCharacter(CharacterClass characterClass)
        {
            return FoundClass(characterClass).characterInformation.isCharacter;
        }

        public Sprite GetIcon(CharacterClass characterClass)
        {
            return FoundClass(characterClass).characterInformation.icon;
        }

        public WeaponConfig GetWeaponConfig(CharacterClass characterClass)
        {
            return FoundClass(characterClass).weapon;
        }

        public IEnumerable<Ability> GetAbilities(CharacterClass characterClass)
        {
            foreach (var ability in FoundClass(characterClass).abilities)
            {
                yield return ability;
            }        
        }

        public PerksProgression GetPerksProgression(CharacterClass characterClass)
        {
            return FoundClass(characterClass).perksProgression;
        }

        public string GetLocalizedName(CharacterClass characterClass)
        {
            foreach(var data in FoundClass(characterClass).localizedData)
            {
                if (PlayerPrefs.GetInt("LocaleKey") == data.language)
                {
                    return data.name;
                }
            }
            return "Пепс";
        }

        public string GetLocalizedDescription(CharacterClass characterClass)
        {
            foreach (var data in FoundClass(characterClass).localizedData)
            {
                if (PlayerPrefs.GetInt("LocaleKey") == data.language)
                {
                    return data.description;
                }
            }
            return "Пепс";
        }

        public Character FoundClass(CharacterClass characterClass)
        {
            foreach (var character in characters)
            {
                if (character.characterClass == characterClass)
                {
                    return character;
                }
            }
            return null;
        }

        public void BuyCharacter(Character character)
        {
            character.characterInformation.bought = true;
        }

        [System.Serializable]
        public class Character
        {
            public CharacterClass characterClass;
            public CharacterInformation characterInformation = null;
            public LocalizedData[] localizedData = null;
            public WeaponConfig weapon = null;
            public Ability[] abilities = null;
            public PerksProgression perksProgression = null;
            public ProgressionStat[] BaseStats;
        }

        [System.Serializable]
        public class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }

        [System.Serializable]
        public class LocalizedData
        {
            public int language = -1;
            public string name = null;
            [TextArea] public string description = null;
        }

        [System.Serializable]
        public class CharacterInformation
        {
            public bool isCharacter = false;
            public bool bought = false;
            public int price = 0;
            public Sprite icon = null;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Perks
{
    [CreateAssetMenu(fileName = "New Perks Progression", menuName = "RPG Time Killer/Perks/New Perks Progression", order = 0)]
    public class PerksProgression : ScriptableObject
    {
        [SerializeField] PerkTypes[] perkTypes;
        Dictionary<PerkType, Dictionary<Perk, bool>> lookupTable = null;

        public Perk GetPerk(PerkType perkType)
        {
            foreach (var choosed in lookupTable[perkType])
            {
                if (!choosed.Value) return choosed.Key;
            }
            return null;
        }

        public IEnumerable<Perk> GetPerks()
        {
            foreach (PerkType perkType in lookupTable.Keys)
            {
                yield return GetPerk(perkType);
            }
        }

        public void ChoosePerk(Perk choosedPerk)
        {
            foreach (PerkTypes perkType in perkTypes)
            {
                foreach (var perk in perkType.perks)
                {
                    if (choosedPerk == perk.perk)
                    {
                        perk.choosed = true;
                        Debug.Log("You Choosed " + choosedPerk);
                        BuildLookup();
                        return;
                    }
                }
            }
        }

        private void BuildLookup()
        {
            lookupTable = new Dictionary<PerkType, Dictionary<Perk, bool>>();

            foreach (PerkTypes perkType in perkTypes)
            {
                var startLookupTable = new Dictionary<Perk, bool>();

                foreach (Perks perks in perkType.perks)
                {
                    startLookupTable[perks.perk] = perks.choosed;
                }

                lookupTable[perkType.perkType] = startLookupTable;
            }
        }

        public void Restart()
        {
            foreach (PerkTypes perkType in perkTypes)
            {
                foreach (var perk in perkType.perks)
                {
                    perk.choosed = false;
                }
            }
            BuildLookup();
        }

        [System.Serializable]
        class PerkTypes
        {
            public PerkType perkType;
            public Perks[] perks;
        }

        [System.Serializable]
        class Perks
        {
            public Perk perk;
            public bool choosed;
        }
    }
}
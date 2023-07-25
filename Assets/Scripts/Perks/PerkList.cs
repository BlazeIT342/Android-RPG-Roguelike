using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Perks
{
    public class PerkList : MonoBehaviour, IModifierProvider
    {
        PerksProgression perksProgression;
        [SerializeField] List<Perk> equippedPerks = new List<Perk>();

        private void Awake()
        {
            perksProgression = GetComponent<BaseStats>().GetCharacterClassStats().GetPerksProgression(GetComponent<BaseStats>().GetCharacterClass());
            perksProgression.Restart();
        }

        public PerksProgression GetPerkProgression()
        {
            return perksProgression;
        }

        public void AddPerkToList(Perk perk)
        {
            equippedPerks.Add(perk);
            if (perk.GetFunction() != null)
            {
                perk.GetFunction().Invoke();
            }
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var perk in equippedPerks)
            {
                var item = perk as IModifierProvider;
                if (item == null) continue;

                foreach (float modifier in item.GetAdditiveModifier(stat))
                {
                    yield return modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (var perk in equippedPerks)
            {
                var item = perk as IModifierProvider;
                if (item == null) continue;

                foreach (float modifier in item.GetPercentageModifier(stat))
                {
                    yield return modifier;
                }
            }
        }
    }
}
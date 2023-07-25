using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class Item : ScriptableObject, IModifierProvider
    {
        [SerializeField] Sprite icon = null;
        [SerializeField] LocalizedData[] localizedData = null;
        [SerializeField] Modifier[] additiveModifiers;
        [SerializeField] Modifier[] percentageModifiers;

        [System.Serializable]
        public class LocalizedData
        {
            public int language = -1;
            public string name = null;
            [TextArea] public string description = null;
        }

        [System.Serializable]
        struct Modifier
        {
            public Stat stat;
            public float value;
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetLocalizedName()
        {
            foreach (var data in localizedData)
            {
                if (PlayerPrefs.GetInt("LocaleKey") == data.language)
                {
                    return data.name;
                }
            }
            return "Пепс";
        }

        public string GetLocalizedDescription()
        {
            foreach (var data in localizedData)
            {
                if (PlayerPrefs.GetInt("LocaleKey") == data.language)
                {
                    return data.description;
                }
            }
            return "Пепс";
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach (var modifier in additiveModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach (var modifier in percentageModifiers)
            {
                if (modifier.stat == stat)
                {
                    yield return modifier.value;
                }
            }
        }
    }
}

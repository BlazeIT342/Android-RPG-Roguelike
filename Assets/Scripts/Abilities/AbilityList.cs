using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public class AbilityList : MonoBehaviour
    {
        [SerializeField] List<Ability> abilities = new List<Ability>();

        public List<Ability> GetAbilities()
        {
            return abilities;
        }
    }
}
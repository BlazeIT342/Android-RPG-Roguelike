using System;
using UnityEngine;
using RPG.Attributes;
using RPG.Stats;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Health Effect", menuName = "RPG Time Killer/Abilities/Effects/New Health Effect", order = 0)]
    public class HealthEffect : EffectStrategy
    {
        [SerializeField] float healthChange;
        [SerializeField] bool dynamicDamage = false;

        public override void StartEffect(AbilityData data, Action finished)
        {
            foreach (var target in data.GetTargets())
            {
                var health = target.GetComponent<Health>();
                if (health)
                {
                    if (dynamicDamage)
                    {
                        healthChange = -data.GetUser().GetComponent<BaseStats>().GetStat(Stat.Damage) * 4;
                    }
                    if (healthChange < 0)
                    {
                        health.TakeDamage(data.GetUser(), -healthChange);
                    }
                    else
                    {
                        health.Heal(healthChange);
                    }
                }
            }
            finished();
        }
    }
}
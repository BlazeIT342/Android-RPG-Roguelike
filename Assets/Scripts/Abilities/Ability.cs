using RPG.Stats;
using UnityEngine;

namespace RPG.Abilities
{
    [CreateAssetMenu(fileName = "Ability", menuName = "RPG Time Killer/Abilities/New Ability", order = 1)]
    public class Ability : Item
    {
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        [SerializeField] float cooldownTime = 0;

        public void Use(GameObject user)
        {
            CooldownStore cooldownStore = user.GetComponent<CooldownStore>();
            if (cooldownStore.GetTimeRemaining(this) > 0) return;

            AbilityData data = new AbilityData(user);

            targetingStrategy.StartTargeting(data, () => TargetAquired(data));
        }

        private void TargetAquired(AbilityData data)
        {
            CooldownStore cooldownStore = data.GetUser().GetComponent<CooldownStore>();
            cooldownStore.StartCooldown(this, cooldownTime);
            foreach (var filterStrategy in filterStrategies)
            {
                data.SetTargets(filterStrategy.Filter(data.GetTargets()));
            }

            foreach (var effect in effectStrategies)
            {
                effect.StartEffect(data, EffectFinished);
            }

        }

        private void EffectFinished()
        {

        }
    }
}
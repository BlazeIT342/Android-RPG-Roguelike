using System;
using System.Collections;
using UnityEngine;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Target Prefab Effect", menuName = "RPG Time Killer/Abilities/Effects/New Target Prefab Effect", order = 0)]
    public class SpawnTargetPrefabEffect : EffectStrategy
    {
        [SerializeField] GameObject prefabToSpawn;
        [SerializeField] float destroyDelay = -1;

        public override void StartEffect(AbilityData data, Action finished)
        {
            data.StartCoroutine(Effect(data, finished));
        }

        private IEnumerator Effect(AbilityData data, Action finished)
        {
            GameObject instance = Instantiate(prefabToSpawn);
            instance.transform.position = data.GetTargetedPoint();
            yield return new WaitForSeconds(destroyDelay);
            Destroy(instance.gameObject);
            finished();
        }
    }
}
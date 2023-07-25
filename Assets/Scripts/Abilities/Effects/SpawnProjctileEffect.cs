using System;
using UnityEngine;
using RPG.Combat;
using RPG.Stats;
using System.Collections;
using RPG.Control;

namespace RPG.Abilities.Effects
{
    [CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "RPG Time Killer/Abilities/Effects/New Spawn Projectile Effect", order = 0)]
    public class SpawnProjctileEffect : EffectStrategy
    {
        [SerializeField] Bullet projectileToSpawn;
        [SerializeField] float damageMultiplier;
        [SerializeField] bool useTargetPoint = false;

        public override void StartEffect(AbilityData data, Action finished)
        {
            GameObject spawnPosition = GameObject.FindGameObjectWithTag("ShotPoint");
            if (useTargetPoint)
            {
                SpawnProjectileForTargetPoint(data, spawnPosition);
            }
            else
            {
                data.StartCoroutine(SpawnProjectilesForTargets(data, spawnPosition));
            }
            finished();
        }

        private void SpawnProjectileForTargetPoint(AbilityData data, GameObject spawnPosition)
        {
            Vector3 direction = data.GetTargetedPoint() - spawnPosition.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Bullet projectile = Instantiate(projectileToSpawn, spawnPosition.transform.position, Quaternion.Euler(0, 0, angle));
            projectile.SetOptions(data.GetUser(), data.GetUser().GetComponent<BaseStats>().GetStat(Stat.Damage) * damageMultiplier);
        }

        private IEnumerator SpawnProjectilesForTargets(AbilityData data, GameObject spawnPosition)
        {
            data.GetUser().GetComponentInChildren<WeaponController>().SetTargeting(true);
            foreach (var target in data.GetTargets())
            {
                if (target == null) continue;
                Vector3 direction = target.transform.position - spawnPosition.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                data.GetUser().GetComponentInChildren<WeaponController>().SetRotationZ(angle);
                Bullet projectile = Instantiate(projectileToSpawn, spawnPosition.transform.position, Quaternion.Euler(0, 0, angle));
                projectile.SetOptions(data.GetUser(), data.GetUser().GetComponent<BaseStats>().GetStat(Stat.Damage) * damageMultiplier);

                yield return new WaitForSeconds(0.1f);
            }
            data.GetUser().GetComponentInChildren<WeaponController>().SetTargeting(false);
        }
    }
}
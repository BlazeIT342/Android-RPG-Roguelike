using RPG.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Auto Targeting", menuName = "RPG Time Killer/Abilities/Targeting/Auto Targeting", order = 0)]
    public class AutoAreaTargeting : TargetingStrategy
    {
        [SerializeField] float radius = 10f;
        [SerializeField] LayerMask layerMask;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            Vector3 targetedPoint = data.GetUser().GetComponentInChildren<WeaponController>().FindClosestEnemy().position;
            data.SetTargetedPoint(targetedPoint);
            data.SetTargets(GetGameObjectsInRadius(targetedPoint));
            finished();
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(point, radius, Vector3.up, 0, layerMask);
            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;
                yield return hit.collider.gameObject;
            }
        }
    }
}
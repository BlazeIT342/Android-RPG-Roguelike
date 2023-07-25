using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "RPG Project/Weapons/New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject, IModifierProvider
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBtwShots = 1f;
        [SerializeField] float reloadTime = 1f;
        [SerializeField] float bulletsInWeapon = 5;
        [SerializeField] float bulletValue = 1;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] Bullet bullet = null;

        const string weaponName = "Weapon";

        public GameObject Spawn(Transform handTransform, Animator animator)
        {
            DestroyOldWeapon(handTransform);

            GameObject weapon = null;
            if (equippedPrefab != null)
            {
                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform handTransform)
        {
            Transform oldWeapon = handTransform.Find(weaponName);
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public bool HasProjectile()
        {
            return bullet != null;
        }

        public void LaunchProjectile(Transform shotPoint, GameObject instigator, Transform weaponPoint)
        {
            Bullet projetileInstance = Instantiate(bullet, shotPoint.position, weaponPoint.rotation);
            projetileInstance.SetOptions(instigator, GetDamage());
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetTimeBtwShots()
        {
            return timeBtwShots;
        }

        public float GetReloadTime()
        {
            return reloadTime;
        }

        public float GetBulletsInWeapon()
        {
            return bulletsInWeapon;
        }

        public float GetBulletValue()
        {
            return bulletValue;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }

        public float GetRange()
        {
            return weaponRange;
        }

        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}
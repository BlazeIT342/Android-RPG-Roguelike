using RPG.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class SetupWeapon : MonoBehaviour
    {
        [SerializeField] Transform handTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;
        WeaponConfig currentWeaponConfig;
        LazyValue<GameObject> currentWeapon;

        public UnityEvent onHitSound;


        private void Awake()
        {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<GameObject>(SetupDefaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
        }

        private GameObject SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private GameObject AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(handTransform, animator);
        }

        public void Hit(Transform shotPoint, Transform weaponRotation)
        {
            currentWeaponConfig.LaunchProjectile(shotPoint, gameObject, weaponRotation);
        }

        public WeaponConfig GetWeaponConfig()
        {
            return currentWeaponConfig;
        }
    }
}
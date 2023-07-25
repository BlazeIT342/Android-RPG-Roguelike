using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using RPG.Combat;
using RPG.Stats;

namespace RPG.Control
{
    public class WeaponController : MonoBehaviour
    { 
        [SerializeField] Transform shotPoint;
        [SerializeField] float bulletsRemain;
        [SerializeField] float bulletValue;
        [SerializeField] LayerMask layerMask;

        PlayerController playerController;
        FloatingJoystick joystick;
        GameObject joystickHandle;
        Vector3 difference;
        new Camera camera;
        Animator gunAnim;


        bool readyToFindEnemy = true;
        bool readyToShoot = true;
        bool canShoot = true;
        float rotationZ;
        bool rightRotation = false;
        bool targeting = false;

        GameObject player;


        private void Awake()
        {
            joystick = GameObject.FindGameObjectWithTag("Joystick Attack").GetComponent<FloatingJoystick>();
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                joystickHandle = player.GetComponent<PlayerController>().GetJoystickAttackHandle();
            }
            gunAnim = GetComponent<Animator>();
            camera = Camera.main;

            if (playerController.GetControlType() == PlayerController.ControlType.PC) joystick.gameObject.SetActive(false);

            bulletsRemain = player.GetComponent<SetupWeapon>().GetWeaponConfig().GetBulletsInWeapon();
        }

        private void Update()
        {
            if (!playerController.GetIsActive()) return;
            GunController();
            Shoot();
        }

        public float GetRotationZ()
        {
            return rotationZ;
        }

        public void SetRotationZ(float rotation)
        {
            if (-90 <= rotation && rotation <= 90)
            {
                rightRotation = false;
            }
            else
            {
                rightRotation = true;
            }
            GetComponent<SpriteRenderer>().flipY = rightRotation;
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        public void SetTargeting(bool targeting)
        {
            this.targeting = targeting;
        }

        public void SetCanShoot(bool canShoot)
        {
            this.canShoot = canShoot;
        }

        private void GunController()
        {
            if (playerController.GetComponent<Animator>().GetBool("run")) gunAnim.SetBool("run", true);
            else gunAnim.SetBool("run", false);

            if (playerController.GetControlType() == PlayerController.ControlType.PC)
            {
                difference = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            else if (playerController.GetControlType() == PlayerController.ControlType.Android && Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f)
            {
                rotationZ = Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg;
            }
            else if (readyToFindEnemy)
            {
                difference = FindClosestEnemy().position - transform.position;
                rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            }
            if (targeting) return;
            SetRotationZ(rotationZ);
        }

        public void Shoot()
        {
            if (!canShoot) return;
            if (readyToShoot)
            {
                if (Input.GetMouseButton(0) && playerController.GetControlType() == PlayerController.ControlType.PC)
                {
                    StartCoroutine(WaitForShoot());
                }
                else if (playerController.GetControlType() == PlayerController.ControlType.Android && joystick.Horizontal != 0 || joystick.Vertical != 0)
                {
                    StartCoroutine(WaitForShoot());
                }
                else if (joystickHandle.activeInHierarchy && joystick.Horizontal == 0 && joystick.Vertical == 0)
                {
                    StartCoroutine(WaitForShoot());
                }
            }
        }

        private IEnumerable<Transform> GetGameObjectsInRadius()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 70, Vector3.up, 0, layerMask);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject.transform;
            }           
        }

        public Transform FindClosestEnemy()
        {
            StartCoroutine(WaitForSearching());
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Transform potentialTarget in GetGameObjectsInRadius())
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
            if (bestTarget == null) return transform;
            return bestTarget;
        }

        private IEnumerator WaitForSearching()
        {
            readyToFindEnemy = false;
            yield return new WaitForSecondsRealtime(0.1f);
            readyToFindEnemy = true;
        }

        public void StartShoot(Transform shotPoint, Transform WeaponTransform)
        {
            player.GetComponent<SetupWeapon>().Hit(shotPoint, WeaponTransform);
        }

        private IEnumerator WaitForShoot()
        {
            WeaponConfig weaponConfig = player.GetComponent<SetupWeapon>().GetWeaponConfig();
            BaseStats baseStats = player.GetComponent<BaseStats>();
            readyToShoot = false;
            StartShoot(shotPoint, transform);
            bulletsRemain -= weaponConfig.GetBulletValue() + baseStats.GetStat(Stat.BulletValue);
            if (bulletsRemain <= 0)
            {
                float calculatedReloadTime = weaponConfig.GetReloadTime() * baseStats.GetStat(Stat.ReloadTime);
                if (calculatedReloadTime < 0.3f)
                {
                    calculatedReloadTime = 0.3f;
                }
                gunAnim.speed /= calculatedReloadTime;
                gunAnim.SetTrigger("reload");
                gunAnim.SetBool("reloading", true);
                yield return new WaitForSecondsRealtime(calculatedReloadTime);
                gunAnim.SetBool("reloading", false);
                gunAnim.ResetTrigger("reload");
                gunAnim.speed = 1;
                bulletsRemain = weaponConfig.GetBulletsInWeapon() + baseStats.GetStat(Stat.BulletsInWeapon);
            }
            else
            {
                float calculatedAttackSpeed = weaponConfig.GetTimeBtwShots() * baseStats.GetStat(Stat.AttackSpeed);
                if (calculatedAttackSpeed < 0.15f) 
                {
                    calculatedAttackSpeed = 0.15f;
                }
                yield return new WaitForSecondsRealtime(calculatedAttackSpeed);
            } 
            readyToShoot = true;
        }
    }
}
using System.Collections;
using System;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "RPG Time Killer/Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float areaAffectRadius;
        [SerializeField] Transform targetingPrefab;

        Transform targetingPrefabInstance = null;

        public override void StartTargeting(AbilityData data, Action finished)
        {
            PlayerController playerController = data.GetUser().GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finished));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finished)
        {
            data.GetUser().GetComponentInChildren<WeaponController>().SetCanShoot(false);
            bool targeting = false;
            while (!data.IsCancelled())
            {
                FloatingJoystick joystick = GameObject.FindGameObjectWithTag("Joystick Attack").GetComponent<FloatingJoystick>();

                if (playerController.GetJoystickAttackHandle().activeInHierarchy && Mathf.Abs(joystick.Horizontal) >= 0.1f || Mathf.Abs(joystick.Vertical) >= 0.1f)
                {
                    Time.timeScale = 0.4f;
                    targeting = true;
                    if (targetingPrefabInstance == null)
                    {
                        targetingPrefabInstance = Instantiate(targetingPrefab);
                    }
                    else
                    {
                        targetingPrefabInstance.gameObject.SetActive(true);
                    }
                    Transform shotPoint = GameObject.FindGameObjectWithTag("ShotPoint").transform;
                    targetingPrefabInstance.localScale = new Vector3(1, areaAffectRadius, 1);
                    targetingPrefabInstance.rotation = Quaternion.Euler(0, 0, data.GetUser().GetComponentInChildren<WeaponController>().GetRotationZ()-90);                   
                    targetingPrefabInstance.position = shotPoint.position;
                }
                if (targeting && !playerController.GetJoystickAttackHandle().activeInHierarchy)
                {
                    Time.timeScale = 1f;
                    Debug.Log("SUCCESS!");
                    data.SetTargetedPoint(targetingPrefabInstance.GetComponentInChildren<BoxCollider2D>().gameObject.transform.position);
                    break;
                }
                yield return null;
            }
            targetingPrefabInstance.gameObject.SetActive(false);
            data.GetUser().GetComponentInChildren<WeaponController>().SetCanShoot(true);
            finished();
        }
    }
}
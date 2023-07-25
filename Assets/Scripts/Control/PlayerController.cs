using UnityEngine;
using RPG.Attributes;
using RPG.Stats;
using RPG.UI.Perks;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] public enum ControlType { PC, Android }
        [SerializeField] ControlType controlType;       
        [SerializeField] float iFramesDuration;
        [SerializeField] int numberOfFlashes;

        GameObject joystickAttackHandle;
        FloatingJoystick joystick;
        Rigidbody2D playerRb;
        WeaponController weaponController;

        Vector2 moveVelocity;
        Vector2 moveInput;

        bool isActive = true;

        private void Awake()
        {
            joystick = GameObject.FindGameObjectWithTag("Joystick Control").GetComponent<FloatingJoystick>();
            joystickAttackHandle = GameObject.FindGameObjectWithTag("Joystick Attack Handle");
            playerRb = GetComponent<Rigidbody2D>();

            if (controlType == ControlType.PC) joystick.gameObject.SetActive(false);
        }

        private void Start()
        {
            weaponController = GameObject.FindGameObjectWithTag("Weapon").GetComponent<WeaponController>();
        }

        private void Update()
        {            
#if UNITY_EDITOR
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    AddExperience(1);
            //}
            if (Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<Health>().Heal(1);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GetComponent<Health>().TakeDamage(gameObject, 1);
            }
#endif
            if (!GetIsActive())
            {
                GetComponent<BoxCollider2D>().enabled = false;
                return;
            }
            UpdateAnimator();
            PlayerMovement();
        }       

        public ControlType GetControlType()
        {
            return controlType;
        }

        private void PlayerMovement()
        {
            if (controlType == ControlType.PC)
            {
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            else if (controlType == ControlType.Android)
            {
                moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            }

            if (moveInput.x != 0 || moveInput.y != 0)
            {
                GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("run", false);
            }

            moveVelocity = moveInput.normalized * GetComponent<BaseStats>().GetStat(Stat.Speed);
        }

        private void UpdateAnimator()
        {
            GetComponent<BoxCollider2D>().enabled = true;
            if (joystickAttackHandle.activeInHierarchy)
            {
                transform.Translate(moveVelocity * Time.deltaTime * 0.33f);
                //playerRb.MovePosition(playerRb.position + moveVelocity * 0.33f * Time.deltaTime);
            }
            else
            {
                transform.Translate(moveVelocity * Time.deltaTime);
                //playerRb.MovePosition(playerRb.position + moveVelocity * Time.deltaTime);
            }

            if (!joystickAttackHandle.activeInHierarchy)
            {
                if (moveInput.x < -0.01f)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (moveInput.x > 0.01f)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else
            {
                if (-90 <= weaponController.GetRotationZ() && weaponController.GetRotationZ() <= 90)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }

        public void StartLevelUp()
        {
            FindObjectOfType<PerkMenuUI>().ShowMenu();
            GetComponent<Animator>().SetBool("levelup", true);
            GetComponent<Animator>().SetTrigger("levelupTrigger");
            SetIsActive(false);
        }

        public void FinishLevelUp()
        {
            SetIsActive(true);
            GetComponent<Animator>().ResetTrigger("levelupTrigger");
            GetComponent<Animator>().SetBool("levelup", false);
        }

        public bool GetIsActive()
        {
            return isActive;
        }

        public void SetIsActive(bool isActive)
        {
            this.isActive = isActive;
        }

        public GameObject GetJoystickAttackHandle()
        {
            return joystickAttackHandle;
        }
    }
}
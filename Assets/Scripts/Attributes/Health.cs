using UnityEngine.Events;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using System.Collections;
using RPG.UI;
using RPG.Control;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 100f;
        [SerializeField] TakeDamageEvent takeDamage;
        public UnityEvent onDie;

        bool experienceGained = false;
        bool healthRevives = false;
        bool invulnerable = false;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        LazyValue<float> healthPoints;
        bool wasDeadLastFrame = false;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void Start()
        {
            healthPoints.ForceInit();
        }

        //private void OnEnable()
        //{
        //    GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        //}

        //private void OnDisable()
        //{
        //    GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        //}

        public bool IsDead()
        {
            return healthPoints.value <= 0;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            if (healthPoints.value < 0) return;
            if (invulnerable) return;
            if (gameObject.CompareTag("Player"))
            {
                StartCoroutine(Invulnerability());
            }
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);     
            if (healthPoints.value == 0)
            {
                onDie.Invoke();
               // AwardExperience(instigator);
                healthPoints.value = -0.01f;
                if (gameObject.CompareTag("Player"))
                {
                    GetComponent<PlayerController>().SetIsActive(false);
                    FindObjectOfType<DeathPanelUI>().ShowDeathPanel();
                }
            }
            else 
            {
                takeDamage.Invoke(damage);
                if (gameObject.CompareTag("Enemy"))
                {
                   //GetComponentInChildren<EnemyHealthBar>().ShowHealthBar();
                }
            }
            UpdateState();
        }

        public void Heal(float healthToRestore)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healthToRestore, GetMaxHealthPoints());
            healthRevives = true;
            UpdateState();
        }

        public bool GetHealthRevives()
        {
            return healthRevives;
        }

        public void UpdateKilledEnemies()
        {
            FindObjectOfType<GameManager>().UpdateKilledEnemies();
        }

        public void SetHealthRevives(bool healthRevives)
        {
            this.healthRevives = healthRevives;
        }

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return healthPoints.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void UpdateState()
        {
            Animator animator = GetComponent<Animator>();
            if (!wasDeadLastFrame && IsDead())
            {
                animator.SetTrigger("die");
            }
            
            if (wasDeadLastFrame && !IsDead())
            {               
                animator.Rebind();
            }
            wasDeadLastFrame = IsDead();

        }
        private void AwardExperience(GameObject instigator)
        {
            if (experienceGained) return;
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            experienceGained = true;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public IEnumerator Invulnerability()
        {
            invulnerable = true;
            Physics2D.IgnoreLayerCollision(6, 7, invulnerable);
            for (int i = 0; i < 3; i++)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                yield return new WaitForSeconds(2f / (3f * 2f));
                GetComponent<SpriteRenderer>().color = Color.white;
                yield return new WaitForSeconds(2f / (3f * 2f));
            }
            invulnerable = false;
            Physics2D.IgnoreLayerCollision(6, 7, invulnerable);

        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            UpdateState();
        }
    }
}
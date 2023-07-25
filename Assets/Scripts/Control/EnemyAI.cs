using RPG.Attributes;
using RPG.Stats;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] GameObject dieParticles;
        [SerializeField] GameObject experienceParticle;
        [SerializeField] float attackRange = 10;
        [SerializeField] float timeBtwAttack = 2;

        Vector3 moveDirection;
        Rigidbody2D enemyRb;
        GameObject player;
        float timeSinceLastAttack = Mathf.Infinity;
        bool isAttacking = false;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            enemyRb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!player.GetComponent<PlayerController>().GetIsActive()) return;
            if (GetComponent<Health>().IsDead()) return;
            timeSinceLastAttack += Time.deltaTime;

            Flipper();

            if (GetIsInRange())
            {
                AttackBehaviour();
            }
            else if (!isAttacking)
            {
                EnemyMovement();
            }
        }

        private void Flipper()
        {
            if (player.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        private void EnemyMovement()
        {
            moveDirection = (player.transform.position - transform.position).normalized;
            //enemyRb.constraints = RigidbodyConstraints2D.None;
            //enemyRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            //enemyRb.MovePosition(transform.position + moveDirection * GetComponent<BaseStats>().GetStat(Stat.Speed) * Time.deltaTime);
            transform.Translate(moveDirection * GetComponent<BaseStats>().GetStat(Stat.Speed) * Time.deltaTime);
        }

        private void AttackBehaviour()
        {
            enemyRb.constraints = RigidbodyConstraints2D.FreezePosition;
            if (timeSinceLastAttack > timeBtwAttack)
            {
                isAttacking = true;
                StartAttack();
                timeSinceLastAttack = 0;
            }
        }

        public void Hit()
        {
            isAttacking = false;
            if (!GetIsInRange()) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (player != null)
            {
                player.GetComponent<Health>().TakeDamage(gameObject, damage);
            }
            isAttacking = false;
        }

        public void DieAction()
        {
            enemyRb.constraints = RigidbodyConstraints2D.FreezeAll;
            GameObject particles = Instantiate(dieParticles, transform.position, Quaternion.identity);
            Destroy(particles, 3);
            Destroy(gameObject);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < attackRange;
        }

        private void StartAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }
    }
}
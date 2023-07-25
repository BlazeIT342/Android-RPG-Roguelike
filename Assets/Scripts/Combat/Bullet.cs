using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Bullet : MonoBehaviour
    {
        GameObject instigator;
        ParticleSystem particleSystemm;

        [SerializeField] LayerMask whatIsSolid;
        [SerializeField] bool transparent = false;
        [SerializeField] float maxLifeTime = 3;
        [SerializeField] float speed = 200;      
        [SerializeField] float lifeAfterImpact = 0f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;

        float damage;

        private void Start()
        {
            if (TryGetComponent(out particleSystemm))
            {
                particleSystemm = GetComponent<ParticleSystem>();
            }
        }

        private void Update()
        {
            if (particleSystemm != null)
            {
                ParticleMovement();
                return;
            }
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        public void SetOptions(GameObject instigator, float damage)
        {
            this.instigator = instigator;
            this.damage = damage + instigator.GetComponent<BaseStats>().GetStat(Stat.Damage);
   
            Destroy(gameObject, maxLifeTime);
        }

        private void Hit()
        {
            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation);
            }

            if (!transparent)
            {
                foreach (GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }
                Destroy(gameObject, lifeAfterImpact);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Health health = collision.GetComponent<Health>();
            if (health == null || health.IsDead()) return;
            if (collision.gameObject == instigator) return;
            health.TakeDamage(instigator, damage);
            //speed = 0f;

            Hit();
        }

        private void ParticleMovement()
        {
            // Получаем массив частиц
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystemm.main.maxParticles];

            // Получаем данные о частицах
            int particleCount = particleSystemm.GetParticles(particles);

            // Проходимся по каждой частице
            for (int i = 0; i < particleCount; i++)
            {
                // Обрабатываем каждую частицу
                ParticleSystem.Particle particle = particles[i];
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                //particle.position += (player.transform.position - particle.position).normalized * speed * Time.deltaTime;
                particle.position = transform.position;
                // Применяем изменения к частице
                particles[i] = particle;
            }

            // Применяем изменения обратно в систему частиц
            particleSystemm.SetParticles(particles, particleCount);
        }
    }
}
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ParticleCollector : MonoBehaviour
    {
        [SerializeField] float force = 10000f;
        ParticleSystem particleSystemm;
        GameObject player = null;
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

        private void Start()
        {
            particleSystemm = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (player != null)
            {
                ConnectToPlayer();
            }
        }

        private void OnParticleTrigger()
        {
            int triggeredParticles = particleSystemm.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

            for (int i = 0; i < triggeredParticles; i++)
            {
                ParticleSystem.Particle particle = particles[i];
                particle.remainingLifetime = 0;
                player.GetComponent<Experience>().GainExperience(1);
                particles[i] = particle;
                Destroy(gameObject);
            }

            particleSystemm.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                player = collision.gameObject;
                particleSystemm.trigger.AddCollider(player.GetComponent<BoxCollider2D>());
            }
        }

        private void ConnectToPlayer()
        {
            // �������� ������ ������
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particleSystemm.main.maxParticles];

            // �������� ������ � ��������
            int particleCount = particleSystemm.GetParticles(particles);

            // ���������� �� ������ �������
            for (int i = 0; i < particleCount; i++)
            {
                // ������������ ������ �������
                ParticleSystem.Particle particle = particles[i];

                particle.position += (player.transform.position - particle.position).normalized * force * Time.deltaTime;

                // ��������� ��������� � �������
                particles[i] = particle;
            }

            // ��������� ��������� ������� � ������� ������
            particleSystemm.SetParticles(particles, particleCount);
        }
    }
}
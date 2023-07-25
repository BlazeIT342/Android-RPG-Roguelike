using UnityEngine;

namespace RPG.Attributes
{
    public class TakeHeart : MonoBehaviour
    {
        [SerializeField] int healthValue;
        Health playerHealth;

        private void Awake()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                HealPlayer(healthValue);
                Destroy(gameObject);
            }
        }

        public void HealPlayer(float healthValue)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            playerHealth.Heal(healthValue);
        }
    }
}
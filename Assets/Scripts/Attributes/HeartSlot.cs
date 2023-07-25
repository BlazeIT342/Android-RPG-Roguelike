using System.Collections;
using UnityEngine;

namespace RPG.Attributes
{
    public class HeartSlot : MonoBehaviour
    {
        [SerializeField] int index;
        Health playerHealth;
        Animator heartAnim;

        private void Awake()
        {

        }

        private void Start()
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            heartAnim = GetComponent<Animator>();
            if (index > playerHealth.GetHealthPoints()) gameObject.SetActive(false);
        }

        private void Update()
        {
            if (index > playerHealth.GetHealthPoints())
            {
                heartAnim.SetBool("Heart dies", true);
            }
            if (playerHealth.GetHealthRevives() && index <= playerHealth.GetHealthPoints())
            {
                StartCoroutine(HeartRevives());
            }
        }

        private IEnumerator HeartRevives()
        {
            heartAnim.SetBool("Heart dies", true);
            heartAnim.SetBool("Heart revives", true);
            yield return new WaitForSecondsRealtime(1.117f);
            playerHealth.SetHealthRevives(false);
            heartAnim.SetBool("Heart revives", false);
            heartAnim.SetBool("Heart dies", false);
        }
    }
}
using UnityEngine.UI;
using UnityEngine;

namespace RPG.Stats
{
    public class PlayerExperienceBar : MonoBehaviour
    {
        [SerializeField] GameObject fillArea;
        Slider playerExperienceSlider;
        Experience experience;

        private void Start()
        {
            playerExperienceSlider = GetComponent<Slider>();
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            if (experience.CurrentExperience() == 0)
            {
                fillArea.SetActive(false);
            }
            else
            {
                fillArea.SetActive(true);
            }
            playerExperienceSlider.maxValue = experience.CalculateMaxExperienceToNextLevel();
            playerExperienceSlider.value = experience.CurrentExperience();
        }
    }
} 
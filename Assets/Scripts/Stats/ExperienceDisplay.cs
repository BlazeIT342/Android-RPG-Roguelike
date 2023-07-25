using UnityEngine;
using TMPro;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;

        private void Start()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<TextMeshProUGUI>().text = string.Format("{0:0}/{1:0}", experience.CurrentExperience(), experience.CalculateMaxExperienceToNextLevel());
        }
    }
}
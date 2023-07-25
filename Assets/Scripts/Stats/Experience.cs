using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;


        public event Action onExperienceGained;

#if UNITY_EDITOR
        //private void Update()
        //{
        //    if (Input.GetKey(KeyCode.E))
        //    {
        //        GainExperience(Time.deltaTime * 1000);                
        //    }
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        GainExperience(56000);
        //    }
        //}
#endif

        public void GainExperience(float experience)
        {         
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetMaxExperiencePrevLevel()
        {
            return GetComponent<BaseStats>().GetStatByPrevLevel(Stat.ExperienceToLevelUp);
        }

        public float GetMaxExperience()
        {
            return GetComponent<BaseStats>().GetStat(Stat.ExperienceToLevelUp);
        }

        public float CalculateMaxExperienceToNextLevel()
        {
            return GetMaxExperience() - GetMaxExperiencePrevLevel();
        }

        public float CurrentExperience()
        {
            return (GetMaxExperiencePrevLevel() - GetExperience()) * -1;
        }

        public float GetExperience()
        {
            return experiencePoints;
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
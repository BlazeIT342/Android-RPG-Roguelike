using RPG.Inventories;
using RPG.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class DeathPanelUI : MonoBehaviour
    {
        [SerializeField] GameObject deathPanel;
        [SerializeField] GameObject perksPanel;
        [SerializeField] TextMeshProUGUI killedEnemies;
        [SerializeField] TextMeshProUGUI surviveTime;
        [SerializeField] TextMeshProUGUI earnedLevels;
        [SerializeField] TextMeshProUGUI reward;

        GameManager gameManager;

        bool started = false;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void ShowDeathPanel()
        {
            perksPanel.SetActive(false);
            deathPanel.SetActive(true);
            StartCoroutine(UpdateUI());
        }

        public void HideDeathPanel()
        {
            deathPanel.SetActive(false);
        }

        public void RestartScene()
        {
            gameManager.GetComponent<Purse>().UpdateBalance(SummarizeReward());
            FindObjectOfType<SavingWrapper>().Save();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMenuScene()
        {
            gameManager.GetComponent<Purse>().UpdateBalance(SummarizeReward());
            FindObjectOfType<SavingWrapper>().Save();
            SceneManager.LoadSceneAsync(0);
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
         
            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
            return formattedTime;
        }

        public IEnumerator UpdateUI()
        {
            StartCoroutine(UpdateElement(killedEnemies, gameManager.GetKilledEnemies()));
            yield return new WaitUntil(() => !started);
            StartCoroutine(UpdateElement(surviveTime, gameManager.GetSurviveTime()));
            yield return new WaitUntil(() => !started);
            StartCoroutine(UpdateElement(earnedLevels, gameManager.GetEarnedLevels()));
            yield return new WaitUntil(() => !started);
            StartCoroutine(UpdateElement(reward, SummarizeReward()));
        }

        public IEnumerator UpdateElement(TextMeshProUGUI text, int value)
        {
            started = true;
            int currentNumber = 0;
            float elapsedTime = 0;
            while (currentNumber < value)
            {
                float increment = value * (elapsedTime / 20);
                currentNumber += Mathf.CeilToInt(increment);

                if (currentNumber > value)
                {
                    currentNumber = value;
                }

                text.text = currentNumber.ToString();
                elapsedTime += Time.deltaTime;
                yield return null;
            }         
            started = false;
        }

        public IEnumerator UpdateElement(TextMeshProUGUI text, float value)
        {
            started = true;
            float currentNumber = 0;
            float elapsedTime = 0;
            while (currentNumber < value)
            {
                float increment = value * (elapsedTime / 20);
                currentNumber += Mathf.CeilToInt(increment);

                if (currentNumber > value)
                {
                    currentNumber = value;
                }

                text.text = FormatTime(currentNumber);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            started = false;
        }

        public int SummarizeReward()
        {
            return (int)(gameManager.GetKilledEnemies() + gameManager.GetSurviveTime() + gameManager.GetEarnedLevels());
        }
    }
}
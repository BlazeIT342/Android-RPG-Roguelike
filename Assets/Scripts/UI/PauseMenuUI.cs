using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu;
        [SerializeField] Button backToMenuButton;

        private void Start()
        {
            FindObjectOfType<SavingWrapper>().Load();
            backToMenuButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                FindObjectOfType<SavingWrapper>().Save();
                SceneManager.LoadSceneAsync(0);
            });
        }

        public void SwitchTime()
        {
            if (Time.timeScale >= 1)
            {
                Time.timeScale = 0;
            }
            else if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
        }
    }
}
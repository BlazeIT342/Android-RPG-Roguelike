using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        private void Start()
        {
            Load();
            Save();
        }

        public void Load()
        {
            FindObjectOfType<SavingWrapper>().Load();
        }

        public void Save()
        {
            FindObjectOfType<SavingWrapper>().Save();
        }

        public void Delete()
        {
            FindObjectOfType<SavingWrapper>().Delete();
        }

        public void StartGame()
        {
            SceneManager.LoadSceneAsync(1);
            Save();
        }

        public void QuitGame()
        {
            Save();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
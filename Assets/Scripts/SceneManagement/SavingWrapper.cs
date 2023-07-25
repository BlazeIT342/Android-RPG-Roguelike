using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string currentSaveKey = "CurrentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;
        [SerializeField] int creditsMenuLevelBuildIndex = 3;

        public void ContinueGame()
        {
            if (!PlayerPrefs.HasKey(currentSaveKey)) return;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }

        public bool CheckForSaves()
        {
            if (GetCurrentSave() == null) return false;
            if (!PlayerPrefs.HasKey(currentSaveKey)) return false;
            if (!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return false;
            return true;
        }

        public void NewGame(string saveFile)
        {
            if (string.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            StartCoroutine(LoadLastScene());
        }

        public void DeleteGame(string saveFile)
        {
            GetComponent<SavingSystem>().Delete(saveFile);
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        public void LoadCreditsMenu()
        {
            StartCoroutine(LoadCreditsMenuScene());
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        //public void SetLoadingScreen(Fader fader)
        //{
        //    if (GetComponent<SavingSystem>().FindBuildIndex(GetCurrentSave()) == 2)
        //    {
        //        fader.SetLoadingScreenCastle();
        //    }
        //    else
        //    {
        //        fader.SetLoadingScreenForest();
        //    }
        //}

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            //SetLoadingScreen(fader);
            yield return fader.FadeOut(fadeOutTime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            //fader.SetLoadingScreenForest();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            //fader.SetLoadingScreenForest();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        private IEnumerator LoadCreditsMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            //fader.SetLoadingScreenCastle();
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(creditsMenuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }
#endif
        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }
    }
}
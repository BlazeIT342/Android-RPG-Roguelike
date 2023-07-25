using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        //[SerializeField] Sprite loadingScreenForest;
        //[SerializeField] Sprite loadingScreenCastle;
        [SerializeField] Image loadingScreen;
        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        //public void SetLoadingScreenForest()
        //{
        //    GetComponent<Image>().sprite = loadingScreenForest;
        //}

        //public void SetLoadingScreenCastle()
        //{
        //    GetComponent<Image>().sprite = loadingScreenCastle;
        //}

        private void Update()
        {

        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1.0f;
        }

        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        public Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while(!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
                yield return null;
            }

        }
    }
}
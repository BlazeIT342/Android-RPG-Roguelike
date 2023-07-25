using UnityEngine;

namespace RPG.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer = null;
        [SerializeField] bool activateOnStart = false;

        private void Start()
        {
            if (uiContainer != null)
            {
                uiContainer.SetActive(activateOnStart);
            }
        }

        public void Toggle()
        {
            if (uiContainer != null)
            {
                uiContainer.SetActive(!uiContainer.activeSelf);
            }
        }
    }
}
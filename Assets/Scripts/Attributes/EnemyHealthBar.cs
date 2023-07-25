using UnityEngine;

namespace RPG.Attributes
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas rootCanvas = null;

        float timeToShow = 1f;
        bool showHealthBar = false;

        private void Update()
        {
            if(Mathf.Approximately(healthComponent.GetFraction(), 0)
            || Mathf.Approximately(healthComponent.GetFraction(), 1)
            || healthComponent.GetHealthPoints() < 0)
            {
                rootCanvas.enabled = false;
                return;
            }
            if (showHealthBar && timeToShow > 0)
            {
                timeToShow -= Time.deltaTime;
                rootCanvas.enabled = true;
            }
            else
            {
                timeToShow = 1f;
                showHealthBar = false;
                rootCanvas.enabled = false;
            }
            
            foreground.localScale = new Vector3(healthComponent.GetFraction(), 1.0f, 1.0f);
        }

        public void ShowHealthBar()
        {
            timeToShow = 1f;
            showHealthBar = true;
        }
    }
}
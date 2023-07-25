using UnityEngine;

namespace RPG.SceneManagement
{
    public class PortalAccessed : MonoBehaviour
    {
        [SerializeField] bool activate = true;

        private void Start()
        {
            gameObject.SetActive(activate);
        }

        public void ChangeState(bool state)
        {
            gameObject.SetActive(state);
        }
    }
}
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;
using UnityEngine;
using RPG.Control;


namespace RPG.SceneManagement
{
    enum DestinationIdentifier { A, B, C, D, E}

    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeInTime = 1.0f;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeWaitTime = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            DontDestroyOnLoad(transform.root.gameObject);
            Fader fader = FindObjectOfType<Fader>();
            //if (sceneToLoad == 1)
            //{
            //    fader.SetLoadingScreenForest();
            //}
            //else if (sceneToLoad == 2)
            //{
            //    fader.SetLoadingScreenCastle();
            //}
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;

            yield return fader.FadeOut(fadeOutTime);

            wrapper.Save();
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            wrapper.Load();

            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            wrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);

            newPlayerController.enabled = true;
            Destroy(gameObject, 0.1f);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal GetOtherPortal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;
                return portal;
            }
            return null;
        }
    }
}
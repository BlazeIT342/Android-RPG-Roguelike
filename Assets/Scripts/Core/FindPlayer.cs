using Cinemachine;
using UnityEngine;

namespace RPG.Core
{
    public class FindPlayer : MonoBehaviour
    {
        PlayerSpawner playerSpawner;

        private void Awake()
        {
            playerSpawner = FindObjectOfType<PlayerSpawner>();
            if (playerSpawner.GetCharacter() != null)
            {
                Instantiate(playerSpawner.GetCharacter());
                print("Spawned " + playerSpawner.GetCharacter());
            }
            GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
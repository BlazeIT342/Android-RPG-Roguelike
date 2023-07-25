using UnityEngine;

namespace RPG.Core
{
    public class RepeatBackground : MonoBehaviour
    {
        [SerializeField] Vector3 backgroundPosition;
        [SerializeField] Vector3 playerPosition;
        [SerializeField] float repeatHeight;
        [SerializeField] float repeatWidth;

        GameObject player;

        private void Start()
        {
            repeatHeight = GetComponent<BoxCollider2D>().size.y / 2;
            repeatWidth = GetComponent<BoxCollider2D>().size.x / 2;
            player = GameObject.FindGameObjectWithTag("Player");
            backgroundPosition = transform.position;
        }

        private void Update()
        {
            RepeatBack();
        }

        private void RepeatBack()
        {
            playerPosition = player.transform.position;

            if (playerPosition.x > backgroundPosition.x + repeatWidth / 2)
            {
                backgroundPosition.x += repeatWidth;
                transform.position = backgroundPosition;
            }
            if (playerPosition.x < backgroundPosition.x - repeatWidth / 2)
            {
                backgroundPosition.x -= repeatWidth;
                transform.position = backgroundPosition;
            }
            if (playerPosition.y > backgroundPosition.y - repeatHeight / 2)
            {
                backgroundPosition.y += repeatHeight;
                transform.position = backgroundPosition;
            }
            if (playerPosition.y < backgroundPosition.y - repeatHeight / 2)
            {
                backgroundPosition.y -= repeatHeight;
                transform.position = backgroundPosition;
            }
        }
    }
}
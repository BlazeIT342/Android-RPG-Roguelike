using UnityEngine;

public class TakeMoney : MonoBehaviour
{
    [SerializeField] float experienceValue;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;

    bool isPlayerNear;
    bool isBlack;

    private void Awake()
    {
        isBlack = false;
        isPlayerNear = false;
    }

    private void Update()
    {
        if (isPlayerNear && isBlack)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
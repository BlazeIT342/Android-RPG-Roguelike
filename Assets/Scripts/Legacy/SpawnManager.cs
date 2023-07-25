using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnRate;
    [SerializeField] private int waveNumber;
    [SerializeField] private int spawnEnemies;
    [SerializeField] private float spawnDelay;

    private Transform playerPosition;

    private void Start()
    {
        InvokeRepeating("SpawnEnemyWave", spawnDelay, spawnRate);
    }

    private void SpawnEnemyWave()
    {
        for (int i = 0; i < spawnEnemies; i++)
        {
            Instantiate(enemyPrefabs[0], GenerateSpawnPosition(), enemyPrefabs[0].transform.rotation);
        }
        waveNumber++;
        spawnEnemies += 2;
    }

    private Vector3 GenerateSpawnPosition()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float spawnRangeX = (Random.Range(100, 140));
        float spawnRangeY = 70f;

        float spawnPosY = Random.Range(playerPosition.position.y - spawnRangeY, playerPosition.position.y + spawnRangeY);

        Vector3 spawnPos1 = new Vector3(playerPosition.position.x - spawnRangeX, spawnPosY, 0);
        Vector3 spawnPos2 = new Vector3(playerPosition.position.x + spawnRangeX, spawnPosY, 0);

        if (Random.Range(0, 100) % 2 == 0) return spawnPos1;
        else return spawnPos2;
    }
}
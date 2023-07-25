using RPG.Attributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int killedEnemies = 0;
    float surviveTime = 0;
    int earnedLevels = 0;

    Health player;

    private void Start()
    {
        killedEnemies = 0;
        surviveTime = 0;
        earnedLevels = 0;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        if (player.IsDead()) return;
        surviveTime += Time.deltaTime;
    }

    public void UpdateKilledEnemies()
    {
        killedEnemies++;
    }

    public void UpdateEarnedLevels()
    {
        earnedLevels++;
    }

    public int GetKilledEnemies()
    {
        return killedEnemies;
    }

    public float GetSurviveTime()
    {
        return surviveTime;
    }

    public int GetEarnedLevels()
    {
        return earnedLevels;
    }
}
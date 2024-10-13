using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreChanged(int newAmount);
    public event ScoreChanged OnScoreChanged;

    private EnemySpawnManager _enemySpawnManager;

    public int score;

    private void Awake()
    {
        score = 0;  
        _enemySpawnManager = FindAnyObjectByType<EnemySpawnManager>();
    }

   


    public void BeatEnemyScore()
    {
        score += 10;
        OnScoreChanged?.Invoke(score);
    }
}

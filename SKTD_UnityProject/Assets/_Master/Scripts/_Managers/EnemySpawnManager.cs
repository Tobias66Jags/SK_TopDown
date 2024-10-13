using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    PoolManager _poolManager;
    private int _lastSpawnIndex = -1;
    [SerializeField] private int _enemiesPerSpawn = 2;
    [SerializeField] private float _spawnInterval = 5f;

    [SerializeField] Transform[] _locations;

    public int maxEnemiesPerRound = 5;

    public int enemiesRemain;
    private int _enemiesCounter;

    public delegate void EnemyChanged(int newAmount);
    public event EnemyChanged OnEnemyChanged;

    private void Awake()
    {
        _poolManager = FindAnyObjectByType<PoolManager>();
    }
    private void Start()
    {
        enemiesRemain = maxEnemiesPerRound;
        StartCoroutine(SpawnEnemiesCoroutine());
    }
    public void SpawnEnemy()
    {
        GameObject enemy = _poolManager.GetEnemy();
        if (enemy != null && _enemiesCounter < maxEnemiesPerRound)
        {
            int spawnIndex = GetRandomSpawnIndex();

            Transform spawnPoint = _locations[spawnIndex];
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);

            _lastSpawnIndex = spawnIndex;
            _enemiesCounter++;
        }
    }
        private int GetRandomSpawnIndex()
    {
        int spawnIndex;

        do
        {
            spawnIndex = Random.Range(0, _locations.Length);
        }
        while (spawnIndex == _lastSpawnIndex);

        return spawnIndex;
    }

    private IEnumerator SpawnEnemiesCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => GameManager.Instance.isPlay);
            SpawnEnemy();         
            yield return new WaitForSeconds(_spawnInterval);

        }
    }

    public void GetEliminatedEnemies()
    {
        enemiesRemain--;
        OnEnemyChanged?.Invoke(enemiesRemain);
    }

}

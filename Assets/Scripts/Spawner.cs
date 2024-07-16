using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    private ObjectPool<Enemy> _enemiesPool;
    private int _enemyPoolCapacity = 10;
    private int _enemyPoolMaxSize = 10;
    private int _repeatRate = 2;

    private void Awake()
    {
        _enemiesPool = new ObjectPool<Enemy>(CreateEnemy, GetFromPool, ReleaseInPool, Destroy, true, _enemyPoolCapacity, _enemyPoolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemyWithRate(_repeatRate));
    }

    private Enemy CreateEnemy()
    {
        SpawnPoint certainSpawnPoint = GetSpawnPoint();
        Enemy certainEnemy = certainSpawnPoint.Enemy;
        Enemy enemy = Instantiate(certainEnemy, certainSpawnPoint.transform.position, Quaternion.identity);
        DefineTarget(enemy, certainSpawnPoint);
        DefineStartPosition(enemy, certainSpawnPoint);

        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
        Debug.Log("out of pool");

        enemy.Removed -= RemoveEnemy;
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.transform.position = enemy.StartPosition;
        StartCoroutine(enemy.Move());
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        Debug.Log("back in pool");
    }

    private SpawnPoint GetSpawnPoint()
    {
        int minSpawnPointIndex = 0;
        int spawnPoint = Random.Range(minSpawnPointIndex, _spawnPoints.Count);

        return _spawnPoints[spawnPoint];
    }

    private IEnumerator SpawnEnemyWithRate(int repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (true)
        {
            yield return wait;

            GetEnemy();
        }
    }

    private void DefineTarget(Enemy enemy, SpawnPoint spawnPoint)
    {
        enemy.GetTarget(spawnPoint.Target);
    }

    private void DefineStartPosition(Enemy enemy, SpawnPoint spawnPoint)
    {
        enemy.GetStartPosition(spawnPoint);
    }

    private void GetEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
        newEnemy.Removed += RemoveEnemy;
    }
}

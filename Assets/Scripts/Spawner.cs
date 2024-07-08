using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    private ObjectPool<Enemy> _enemiesPool;
    private int _enemyPoolCapacity = 25;
    private int _enemyPoolMaxSize = 25;
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
        float randomYAngle = Random.Range(0f, 360f);
        SpawnPoint ceratainSpawnPoint = GetSpawnPoint();
        Enemy certainEnemy = DefineEnemy(ceratainSpawnPoint);
        Enemy enemy = Instantiate(certainEnemy, ceratainSpawnPoint.transform.position, Quaternion.Euler(0f, randomYAngle, 0f));
        DefineTarget(enemy, ceratainSpawnPoint);
        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
    }

    private void GetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);

        StartCoroutine(enemy.Move());
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private Enemy DefineEnemy(SpawnPoint spawnPoint)
    {
         return spawnPoint.Enemy;
    }

    private SpawnPoint GetSpawnPoint()
    {
        int spawnPoint = Random.Range(0, _spawnPoints.Count);

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
        enemy.Target = spawnPoint.Target;
    }

    private void GetEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
    }
}

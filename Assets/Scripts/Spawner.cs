using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _spawnPoints;

    private ObjectPool<Enemy> _enemiesPool;
    private int _enemyPoolCapacity = 20;
    private int _enemyPoolMaxSize = 20;
    private int _repeatRate = 2;

    private void Awake()
    {
        _enemiesPool = new ObjectPool<Enemy>(CreateEnemy, ActionOnGet, ActionOnRelease, Destroy, true, _enemyPoolCapacity, _enemyPoolMaxSize);
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
        enemy.Removed -= RemoveEnemy;
    }

    private void ActionOnGet(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.transform.position = enemy.StartPosition;
        StartCoroutine(enemy.Move());
    }

    private void ActionOnRelease(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private SpawnPoint GetSpawnPoint()
    {
        int spawnPoint;

        return _spawnPoints[spawnPoint = Random.Range(0, _spawnPoints.Count)];
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
        enemy.RecieveTarget(spawnPoint.Target);
    }

    private void DefineStartPosition(Enemy enemy, SpawnPoint spawnPoint)
    {
        enemy.RecieveStartPosition(spawnPoint);
    }

    private void GetEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
        newEnemy.Removed += RemoveEnemy;
    }
}

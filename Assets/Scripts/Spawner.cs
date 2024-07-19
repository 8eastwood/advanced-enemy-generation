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
        _enemiesPool = new ObjectPool<Enemy>(
            createFunc: () => CreateEnemy(),
            actionOnGet: (enemy) => Initialization(enemy),
            actionOnRelease: (enemy) => Disable(enemy),
            actionOnDestroy: (enemy) => Destroy(enemy),
            collectionCheck: true,
            defaultCapacity: _enemyPoolCapacity,
            maxSize: _enemyPoolMaxSize);
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
        enemy.RecieveTarget(certainSpawnPoint.Target);
        enemy.RecieveStartPosition(certainSpawnPoint);

        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);
        enemy.Removed -= RemoveEnemy;
    }

    private void Initialization(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.transform.position = enemy.StartPosition;
    }

    private void Disable(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private SpawnPoint GetSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)];
    }

    private IEnumerator SpawnEnemyWithRate(int repeatRate)
    {
        var wait = new WaitForSeconds(repeatRate);

        while (enabled)
        {
            yield return wait;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
        newEnemy.Removed += RemoveEnemy;
    }
}

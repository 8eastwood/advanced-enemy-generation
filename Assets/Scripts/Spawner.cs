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

        return enemy;
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _enemiesPool.Release(enemy);

        enemy.Removed -= RemoveEnemy;
    }

    private void GetFromPool(Enemy enemy)
    {
        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (enemy == _spawnPoints[i].Enemy)
            {
                DefineTarget(enemy, _spawnPoints[i]);
                enemy.transform.position = new Vector3(_spawnPoints[i].transform.position.x, _spawnPoints[i].transform.position.y, _spawnPoints[i].transform.position.z);
            }
        }

        enemy.GetComponent<Rigidbody>().velocity = Vector3.zero;
        enemy.gameObject.SetActive(true);
        StartCoroutine(enemy.Move());
        Debug.Log("out of pool");
    }

    private void ReleaseInPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
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

    private void GetEnemy()
    {
        Enemy newEnemy = _enemiesPool.Get();
        newEnemy.Removed += RemoveEnemy;
    }
}

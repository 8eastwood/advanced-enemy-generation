using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Target _targetPrefab;

    public Enemy Enemy => _enemyPrefab;  
    public Target Target => _targetPrefab;  
}

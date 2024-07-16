using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Target _target;

    public Enemy Enemy => _enemy;  
    public Target Target => _target;  
}

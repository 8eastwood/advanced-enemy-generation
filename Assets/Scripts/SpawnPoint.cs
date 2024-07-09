using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Target _target;

    public Enemy Enemy { get { return _enemy; } }
    public Target Target { get { return _target; } }
}

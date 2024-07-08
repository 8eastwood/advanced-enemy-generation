using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] public Enemy Enemy;
    [SerializeField] private Target target;

    public Target Target { get { return target; } }
}

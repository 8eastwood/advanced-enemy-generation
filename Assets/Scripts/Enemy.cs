using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private int _delay = 0;

    private float _speed = 5f;
    private Target _target = null;
    private Vector3 _startPosition;

    public event Action<Enemy> Removed;

    public Vector3 StartPosition => _startPosition;

    private void Start()
    {
        StartCoroutine(Move());
    }

    private void Update()
    {
        if (_target != null)
        {
            transform.LookAt(_target.transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Target target))
        {
            StartCoroutine(Destroy(_delay));
        }
    }

    public void RecieveTarget(Target target)
    {
        _target = target;
    }

    public void RecieveStartPosition(SpawnPoint spawnPoint)
    {
        _startPosition = spawnPoint.transform.position;
    }

    private IEnumerator Destroy(int delay)
    {
        yield return new WaitForSeconds(delay);
        Removed?.Invoke(this);
    }

    private IEnumerator Move()
    {
        if (_target != null)
        {
            while (isActiveAndEnabled)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position,
                    _speed * Time.deltaTime);

                yield return null;
            }
        }
    }
}

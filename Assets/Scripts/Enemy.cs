using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    private float _speed = 5f;
    private Target _target;
    private Vector3 _startPosition;

    public Vector3 StartPosition => _startPosition;

    public event Action<Enemy> Removed;

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
            Debug.Log("collision happend");
            StartCoroutine(Destroy());
        }
    }

    public IEnumerator Move()
    {
        while (isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            yield return null;
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

    private IEnumerator Destroy()
    {
        yield return null;
        Removed?.Invoke(this);
    }
}

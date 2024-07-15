using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    private float _speed = 5f;
    private Target _target;

    public event Action<Enemy> Removed;

    private void Update()
    {
        transform.LookAt(_target.transform.position);
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

    public void GetTarget(Target target)
    {
        _target = target;
    }

    private IEnumerator Destroy()
    {
        yield return null;
        Removed?.Invoke(this);

    }
}

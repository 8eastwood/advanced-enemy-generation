using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 5f;
    private Target _target;

    private void FixedUpdate()
    {
        transform.LookAt(_target.transform.position);
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
}

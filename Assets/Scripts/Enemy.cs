using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 5f;
    public Target Target;

    private void FixedUpdate()
    {
        transform.LookAt(Target.transform.position);
    }

    public IEnumerator Move()
    {
        while (isActiveAndEnabled)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, _speed * Time.deltaTime);

            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 3f;

    public IEnumerator Move()
    {
        while (isActiveAndEnabled)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);

            yield return null;
        }
    }
}

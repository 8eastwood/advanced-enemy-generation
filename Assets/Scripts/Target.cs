using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Target target;

    private float _speed = 1f;

    private void Start()
    {
        StartCoroutine(TartgetMove());
    }

    public IEnumerator TartgetMove()
    {
        while (true)
        {
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);

            yield return null;
        }
    }
}

using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private DestinationPoint _destinationPoint;

    private float _speed = 5f;

    private void Start()
    {
        StartCoroutine(TartgetMove());
    }

    public IEnumerator TartgetMove()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destinationPoint.transform.position, _speed * Time.deltaTime);
            transform.LookAt(_destinationPoint.transform.position);

            yield return null;
        }
    }
}

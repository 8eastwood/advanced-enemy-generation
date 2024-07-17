using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private Transform[] _wayPoints;

    private float _speed = 5f;
    private int _currentWayPoint = 0;

    private void Start()
    {
        StartCoroutine(TartgetMove());
    }

    private void Update()
    {
        transform.LookAt(_wayPoints[_currentWayPoint].transform.position);
    }

    public IEnumerator TartgetMove()
    {
        while (true)
        {
            if (transform.position == _wayPoints[_currentWayPoint].position)
            {
                _currentWayPoint = (_currentWayPoint + 1) % _wayPoints.Length;
            }

            transform.position = Vector3.MoveTowards(transform.position, _wayPoints[_currentWayPoint].transform.position, _speed * Time.deltaTime);

            yield return null;
        }
    }
}

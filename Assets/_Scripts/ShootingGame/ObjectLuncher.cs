using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLuncher : MonoBehaviour
{
    [SerializeField] private float _lunchOffsetSeconds = 1f;

    [SerializeField] private float _minXDegree = -15f;
    [SerializeField] private float _maxXDegree = 30f;

    [SerializeField] private float _minZDegree = -10f;
    [SerializeField] private float _maxZDegree = 10f;

    private Stack<GameObject> _objectPullStack = new Stack<GameObject>();

    private float elapsedTime = 1f;

    private void Awake()
    {
        elapsedTime = _lunchOffsetSeconds;


    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= _lunchOffsetSeconds)
        {
            elapsedTime -= _lunchOffsetSeconds;
            GetRandomDegreeInRange();
        }
    }

    private void GetRandomDegreeInRange()
    {
        float xDegree = Random.Range(_minXDegree, _maxXDegree);
        float zDegree = Random.Range(_minZDegree, _maxZDegree);

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.rotation = Quaternion.Euler(xDegree, zDegree, 0f);
    }
}
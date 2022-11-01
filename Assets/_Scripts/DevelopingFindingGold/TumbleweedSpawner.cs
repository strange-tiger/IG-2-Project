using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedSpawner : MonoBehaviour
{
    [SerializeField] private float _tumbleweedSpawnOffsetTime = 5f;
    [SerializeField] private Transform _spawnPosition;
    private float _elapsedTime = 0f;

    private Stack<GameObject> _tumbleweedStack = new Stack<GameObject>();

    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;

    private void Awake()
    {
        TumbleweedMovement[] _tumbleweeds = GetComponentsInChildren<TumbleweedMovement>();
        foreach(TumbleweedMovement tumbleweed in _tumbleweeds)
        {
            tumbleweed.gameObject.SetActive(false);
            _tumbleweedStack.Push(tumbleweed.gameObject);
        }
    }

    private void FixedUpdate()
    {
        _elapsedTime += Time.fixedDeltaTime;
        if(_elapsedTime >= _tumbleweedSpawnOffsetTime)
        {
            GameObject tumbleweed = _tumbleweedStack.Pop();
            tumbleweed.transform.position = _spawnPosition.position;
            tumbleweed.transform.rotation = Quaternion.Euler(ZERO_VECTOR);
            tumbleweed.SetActive(true);

            _elapsedTime -= _tumbleweedSpawnOffsetTime;
        }
    }

    public void ReturnToTumbleweedStack(GameObject tumbleweed)
    {
        _tumbleweedStack.Push(tumbleweed);
    }
}

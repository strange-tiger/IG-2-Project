using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TumbleweedSpawner : MonoBehaviourPun
{
    [SerializeField] private float _tumbleweedSpawnOffsetTime = 5f;

    [SerializeField] private float _spawnPositionOffset;
    [SerializeField] private Transform _spawnPosition;
    private float _elapsedTime = 0f;

    private Stack<GameObject> _tumbleweedStack = new Stack<GameObject>();

    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;

    private void Awake()
    {
        if(photonView.IsMine)
        {
            Rigidbody[] _tumbleweeds = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody tumbleweed in _tumbleweeds)
            {
                tumbleweed.gameObject.SetActive(false);
                _tumbleweedStack.Push(tumbleweed.gameObject);
                tumbleweed.GetComponent<Tumbleweed>().enabled = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            _elapsedTime += Time.fixedDeltaTime;
            if (_elapsedTime >= _tumbleweedSpawnOffsetTime)
            {
                SetRandomSpawnPosition();

                GameObject tumbleweed = _tumbleweedStack.Pop();
                tumbleweed.transform.position = _spawnPosition.position;
                tumbleweed.transform.rotation = Quaternion.Euler(ZERO_VECTOR);
                tumbleweed.SetActive(true);

                _elapsedTime -= _tumbleweedSpawnOffsetTime;
            }
        }
    }

    private void SetRandomSpawnPosition()
    {
        float randomXOffset = Random.Range(-_spawnPositionOffset, _spawnPositionOffset);

        float randomZRange = Mathf.Sqrt(Mathf.Pow(_spawnPositionOffset, 2) - Mathf.Pow(randomXOffset, 2));
        float randomZOffset = Random.Range(-randomZRange, randomZRange);
        _spawnPosition.position = new Vector3(transform.position.x + randomXOffset, 
            _spawnPosition.position.y, transform.position.z + randomZOffset);
    }

    public void ReturnToTumbleweedStack(GameObject tumbleweed)
    {
        if(photonView.IsMine)
        {
            _tumbleweedStack.Push(tumbleweed);
        }
    }
}

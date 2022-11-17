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

    private Rigidbody[] _tumbleweedPoll;
    private int _tumbleweedCount;
    private int _currentTumbelweed = 0;

    private readonly static Vector3 ZERO_VECTOR = Vector3.zero;

    private void Awake()
    {
        _tumbleweedPoll = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody tumbleweed in _tumbleweedPoll)
        {
            tumbleweed.gameObject.SetActive(false);
            tumbleweed.GetComponent<Tumbleweed>().enabled = true;
        }
        _tumbleweedCount = _tumbleweedPoll.Length;
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
        {
            _elapsedTime += Time.fixedDeltaTime;
            if (_elapsedTime >= _tumbleweedSpawnOffsetTime)
            {
                SetRandomSpawnPosition();

                GameObject tumbleweed = _tumbleweedPoll[_currentTumbelweed].gameObject;
                tumbleweed.transform.position = _spawnPosition.position;
                tumbleweed.transform.rotation = Quaternion.Euler(ZERO_VECTOR);
                tumbleweed.GetComponent<Tumbleweed>().photonView.RPC("ActiveSelf", RpcTarget.All);

                _currentTumbelweed = (_currentTumbelweed + 1) % _tumbleweedCount;

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
}

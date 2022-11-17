using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoldBoxSpawner : MonoBehaviourPun
{
    [SerializeField] private GameObject _spawnPositionParent;
    private Transform[] _spawnPositions;

    [SerializeField] private GameObject _goldBoxParent;

    private GoldBoxSencer[] _goldBoxPoll;
    private int _goldBoxCount;
    private int _currentGoldBox = 0;

    private void Awake()
    {
        _spawnPositions = _spawnPositionParent.GetComponentsInChildren<Transform>();

        _goldBoxPoll = _goldBoxParent.GetComponentsInChildren<GoldBoxSencer>();
        foreach(GoldBoxSencer goldBox in _goldBoxPoll)
        {
            goldBox.gameObject.SetActive(false);
            goldBox.GetComponentInChildren<GoldBoxInetraction>().
                OnGiveGold.AddListener(SpawnGoldBoxInRandomPosition);
        }
        _goldBoxCount = _goldBoxPoll.Length;

        SpawnGoldBoxInRandomPosition();
    }

    private void SpawnGoldBoxInRandomPosition()
    {
        int positionIndex = Random.Range(1, _spawnPositions.Length);
        GameObject goldBox = _goldBoxPoll[_currentGoldBox].gameObject;
        goldBox.transform.position = _spawnPositions[positionIndex].position;
        goldBox.transform.rotation = _spawnPositions[positionIndex].rotation;
        goldBox.SetActive(true);
        _currentGoldBox = (_currentGoldBox + 1) % _goldBoxCount;
        //photonView.RPC(nameof(SpawnGoldBox), RpcTarget.All);
    }
    
    [PunRPC]
    private void SpawnGoldBox()
    {
        if(PhotonNetwork.IsMasterClient)
        {
        }
    }

    public void ReturnToPoll(GameObject goldBox)
    {
        goldBox.transform.parent = _goldBoxParent.transform;
    }
}

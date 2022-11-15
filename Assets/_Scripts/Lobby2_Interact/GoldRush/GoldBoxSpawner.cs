using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnPositionParent;
    private Transform[] _spawnPositions;

    [SerializeField] private GameObject _goldBoxParent;
    private Queue<GameObject> _goldBoxQueue = new Queue<GameObject>();

    private void Awake()
    {
        _spawnPositions = _spawnPositionParent.GetComponentsInChildren<Transform>();

        foreach(GoldBoxInetraction goldBox in 
            _goldBoxParent.GetComponentsInChildren<GoldBoxInetraction>())
        {
            _goldBoxQueue.Enqueue(goldBox.gameObject);
            goldBox.gameObject.SetActive(false);
            goldBox.GetComponent<GoldBoxInetraction>().
                OnGiveGold.AddListener(SpawnGoldBoxInRandomPosition);
        }

        SpawnGoldBoxInRandomPosition();
    }

    private void SpawnGoldBoxInRandomPosition()
    {
        int positionIndex = Random.Range(1, _spawnPositions.Length);
        GameObject goldBox = _goldBoxQueue.Dequeue();
        goldBox.transform.position = _spawnPositions[positionIndex].position;
        goldBox.transform.rotation = _spawnPositions[positionIndex].rotation;
        goldBox.SetActive(true);
    }

    public void ReturnToPoll(GameObject goldBox)
    {
        _goldBoxQueue.Enqueue(goldBox);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _member;

    [SerializeField]
    private int _setPosition;

    private GameObject[] _firstBattle = new GameObject[2];
    private GameObject[] _secondBattle = new GameObject[2];
    private GameObject[] _finalBattle = new GameObject[2];

    private void OnEnable()
    {
        _member[0].transform.position = new Vector3(-_setPosition, 0, 0);

        _member[1].transform.position = new Vector3(_setPosition, 0, 0);

        _member[2].transform.position = new Vector3(-_setPosition, 0, 0);
        _member[3].transform.position = new Vector3(_setPosition, 0, 0);

        _firstBattle[0] = _member[0];
        _firstBattle[1] = _member[1];
        _secondBattle[0] = _member[2];
        _secondBattle[1] = _member[3];
    }

    void Start()
    {
        for (int i = 0; i < _firstBattle.Length; i++)
        {
            _firstBattle[i].SetActive(true);
        }
    }

    void Update()
    {
        if (_firstBattle[0].activeSelf == false)
        {
            _firstBattle[0] = _firstBattle[1];


        }
        if (_firstBattle[1].activeSelf == false)
        {
            _firstBattle[0] = _firstBattle[0];


        }
    }

    private void OnDisable()
    {
        _member[0].transform.position = Vector3.zero;
        _member[1].transform.position = Vector3.zero;
        _member[2].transform.position = Vector3.zero;
        _member[3].transform.position = Vector3.zero;
    }
}

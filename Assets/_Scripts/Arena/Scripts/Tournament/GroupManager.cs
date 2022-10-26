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

    [SerializeField]
    private AIDeath[] _aIDeath;

    private GameObject[] _finalBattle = new GameObject[2];

    private bool _isFirstBattle;
    private bool _isSecondBattle;
    private bool _isFinelBattle;

    
    
    private void OnEnable()
    {
        _member[0].transform.position = new Vector3(-_setPosition, 0, 0);
        _member[1].transform.position = new Vector3(_setPosition, 0, 0);
        _member[2].transform.position = new Vector3(-_setPosition, 0, 0);
        _member[3].transform.position = new Vector3(_setPosition, 0, 0);
    }

    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            _member[i].SetActive(true);
        }

        for (int i = 0; i < _aIDeath.Length; i++)
        {
            _aIDeath[i].DeathAI.RemoveListener(SomeAIDied);
            _aIDeath[i].DeathAI.AddListener(SomeAIDied);
        }
    }

    void Update()
    {
        if ((_member[0].activeSelf == false || _member[1].activeSelf == false) && !_isFirstBattle)
        {
            if (_member[0].activeSelf)
            {
                _finalBattle[0] = _member[0];
                _member[0].transform.position = new Vector3(-_setPosition, 0, 0);
                _member[0].SetActive(false);
            }
            else if (_member[1].activeSelf)
            {
                _finalBattle[0] = _member[1];
                _member[1].transform.position = new Vector3(_setPosition, 0, 0);
                _member[1].SetActive(false);
            }

            Invoke("SecondBattle", 2f);
        }

        if ((_member[2].activeSelf == false || _member[3].activeSelf == false) && _member[0].activeSelf == false && _member[1].activeSelf == false && _isSecondBattle)
        {
            if (_member[2].activeSelf)
            {
                _finalBattle[1] = _member[2];
                _member[2].transform.position = new Vector3(-_setPosition, 0, 0);
                _member[2].SetActive(false);
            }
            else if (_member[3].activeSelf)
            {
                _finalBattle[1] = _member[3];
                _member[3].transform.position = new Vector3(_setPosition, 0, 0);
                _member[3].SetActive(false);
            }
            _isFinelBattle = true;
        }

        if (_member[0].activeSelf == false && _member[1].activeSelf == false && _member[2].activeSelf == false && _member[3].activeSelf == false && _isFinelBattle)
        {
            for (int i = 0; i < 2; ++i)
            {
                _finalBattle[i].SetActive(true);

                if (i == 1)
                {
                    _isFirstBattle = true;
                }
            }
        }
    }

    private void OnDisable()
    {
        _member[0].transform.position = Vector3.zero;
        _member[1].transform.position = Vector3.zero;
        _member[2].transform.position = Vector3.zero;
        _member[3].transform.position = Vector3.zero;
    }

    private void SomeAIDied(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void SecondBattle()
    {
        for (int i = 2; i < 4; ++i)
        {
            _member[i].SetActive(true);

            if (i == 3)
            {
                _isFirstBattle = true;
                _isSecondBattle = true;
            }
        }
    }
}

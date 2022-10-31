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
    private bool _isWinnerIndex;

    private int _winnerIndex;
    private int _firstWinnerIndex;
    private int _secondWinnerIndex;
    public int WinnerIndex { get { return _winnerIndex; } private set { _winnerIndex = value; } }

    private bool _isDraw;
    public bool IsDraw { get { return _isDraw; } private set { _isDraw = value; } }


    void Start()
    {
        // �ذ�� 1 ��ġ ����
        _member[0].transform.position = new Vector3(-_setPosition, -2, 0);
        _member[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[1].transform.position = new Vector3(_setPosition, -2, 0);
        _member[1].transform.rotation = Quaternion.Euler(0, -90, 0);

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
        if (!_isDraw)
        {


            // �ذ�� 1
            if ((_member[0].activeSelf == false || _member[1].activeSelf == false) && !_isFirstBattle)
            {
                if (_member[0].activeSelf == false && _member[1].activeSelf == false)
                {
                    _isDraw = true;
                }

                if (_member[0].activeSelf)
                {
                    _firstWinnerIndex = 0;
                    _finalBattle[0] = _member[0];
                    _member[0].transform.position = new Vector3(-_setPosition, -2, 0);
                    _member[0].SetActive(false);
                }

                else if (_member[1].activeSelf)
                {
                    _firstWinnerIndex = 1;
                    _finalBattle[0] = _member[1];
                    _member[1].transform.position = new Vector3(-_setPosition, -2, 0);
                    _member[1].SetActive(false);
                }

                Invoke("SecondBattle", 2f);
            }

            // �ذ�� 2
            if ((_member[2].activeSelf == false || _member[3].activeSelf == false) && _member[0].activeSelf == false && _member[1].activeSelf == false && _isSecondBattle)
            {
                if (_member[2].activeSelf == false && _member[3].activeSelf == false)
                {
                    _isDraw = true;
                }

                if (_member[2].activeSelf)
                {
                    _secondWinnerIndex = 2;
                    _finalBattle[1] = _member[2];
                    _member[2].transform.position = new Vector3(_setPosition, -2, 0);
                    _member[2].SetActive(false);
                }

                else if (_member[3].activeSelf)
                {
                    _secondWinnerIndex = 3;
                    _finalBattle[1] = _member[3];
                    _member[3].transform.position = new Vector3(_setPosition, -2, 0);
                    _member[3].SetActive(false);
                }

                Invoke("FinalBattle", 2f);

            }

            if (_member[0].activeSelf == false && _member[1].activeSelf == false && _member[2].activeSelf == false && _member[3].activeSelf == false && _isFinelBattle == true)
            {
                _finalBattle[0].SetActive(true);
                _finalBattle[1].SetActive(true);

                _isWinnerIndex = true;
            }

            if (_finalBattle[0].activeSelf == false && _finalBattle[0].activeSelf == false)
            {
                _isDraw = true;
            }

            if (_isWinnerIndex)
            {
                SendWinnerIndex();
            }
        }
    }

    // ���� AI
    private void SomeAIDied(GameObject obj)
    {
        obj.SetActive(false);
    }

    // �ذ�� 2 ��ġ ����
    private void SecondBattle()
    {
        _member[2].transform.position = new Vector3(-_setPosition, -2, 0);
        _member[2].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[3].transform.position = new Vector3(_setPosition, -2, 0);
        _member[3].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[2].SetActive(true);
        _member[3].SetActive(true);

        _isFirstBattle = true;
        _isSecondBattle = true;
    }

    // �����
    private void FinalBattle()
    {
        _isSecondBattle = false;
        _isFinelBattle = true;

        _finalBattle[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _finalBattle[1].transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    private void SendWinnerIndex()
    {
        if (_isFirstBattle == true && _isSecondBattle == false && _isFinelBattle == true && _finalBattle[0].activeSelf == false || _finalBattle[1].activeSelf == false)
        {
            if (_finalBattle[0].activeSelf)
            {
                _winnerIndex = _firstWinnerIndex;
            }
            else if (_finalBattle[1].activeSelf)
            {
                _winnerIndex = _secondWinnerIndex;
            }
            Debug.Log(_winnerIndex);
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

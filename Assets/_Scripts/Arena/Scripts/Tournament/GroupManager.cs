using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroupManager : MonoBehaviour
{
    [SerializeField] private int _setPosition;

    private GameObject[] _member = new GameObject[4];
    public GameObject[] Member { get { return _member; } }

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

    private int _randomIndex;

    private void OnEnable()
    {
        List<int> _memberIndexList = new List<int>();

        for (int i = 0; i < _member.Length; ++i)
        {
            _randomIndex = Random.Range(0, 4);

            for (int j = 0; j < _memberIndexList.Count;)
            {
                if (_memberIndexList[j] == _randomIndex)
                {
                    j = 0;
                    _randomIndex = Random.Range(0, 4);
                }
                else
                {
                    ++j;
                }
            }
            _memberIndexList.Add(_randomIndex);
        }
        Debug.Log($"{_memberIndexList[0]}, {_memberIndexList[1]}, {_memberIndexList[2]}, {_memberIndexList[3]}");
        for (int i = 0; i < _member.Length; ++i)
        {
            int index;
            index = _memberIndexList[i];
            _member[i] = transform.GetChild(index).gameObject;
        }
    }

    void Start()
    {
        // ÁØ°á½Â 1 À§Ä¡ ¼ÂÆÃ
        _member[0].transform.position = new Vector3(-_setPosition, -2f, 0);
        _member[0].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[1].transform.position = new Vector3(_setPosition, -2f, 0);
        _member[1].transform.rotation = Quaternion.Euler(0, -90, 0);

        for (int i = 0; i < 2; i++)
        {
            _member[i].SetActive(true);
        }
    }

    void Update()
    {
        if (!_isDraw)
        {
            // ÁØ°á½Â 1
            if ((_member[0].activeSelf == false || _member[1].activeSelf == false) && !_isFirstBattle)
            {
                if (_member[0].activeSelf)
                {
                    _firstWinnerIndex = 0;
                    _finalBattle[0] = _member[0];
                    _member[0].transform.position = new Vector3(-_setPosition, -2f, 0);
                    _member[0].SetActive(false);
                }

                else if (_member[1].activeSelf)
                {
                    _firstWinnerIndex = 1;
                    _finalBattle[0] = _member[1];
                    _member[1].transform.position = new Vector3(-_setPosition, -2f, 0);
                    _member[1].SetActive(false);
                }

                Invoke("SecondBattle", 2f);

                _isFirstBattle = true;
            }

            // ÁØ°á½Â 2
            if ((_member[2].activeSelf == false || _member[3].activeSelf == false) && _member[0].activeSelf == false && _member[1].activeSelf == false && _isSecondBattle)
            {
                if (_member[2].activeSelf)
                {
                    _secondWinnerIndex = 2;
                    _finalBattle[1] = _member[2];
                    _member[2].transform.position = new Vector3(_setPosition, -2f, 0);
                    _member[2].SetActive(false);
                }

                else if (_member[3].activeSelf)
                {
                    _secondWinnerIndex = 3;
                    _finalBattle[1] = _member[3];
                    _member[3].transform.position = new Vector3(_setPosition, -2f, 0);
                    _member[3].SetActive(false);
                }

                Invoke("FinalBattle", 2f);

                _isSecondBattle = false;
            }

            if (_member[0].activeSelf == false && _member[1].activeSelf == false && _member[2].activeSelf == false && _member[3].activeSelf == false && _isFinelBattle == true)
            {
                _finalBattle[0].SetActive(true);
                _finalBattle[1].SetActive(true);

                _isWinnerIndex = true;
            }

            if (_isWinnerIndex)
            {
                if (_finalBattle[0].activeSelf == false || _finalBattle[1].activeSelf == false)
                {
                    SendWinnerIndex();
                    Debug.Log(WinnerIndex);
                }
            }
        }
    }

    // Á×Àº AI
    private void SomeAIDied(GameObject obj)
    {
        obj.SetActive(false);
    }

    // ÁØ°á½Â 2 À§Ä¡ ¼ÂÆÃ
    private void SecondBattle()
    {
        _member[2].transform.position = new Vector3(-_setPosition, -2f, 0);
        _member[2].transform.rotation = Quaternion.Euler(0, 90, 0);
        _member[3].transform.position = new Vector3(_setPosition, -2f, 0);
        _member[3].transform.rotation = Quaternion.Euler(0, -90, 0);

        _member[2].SetActive(true);
        _member[3].SetActive(true);


        _isSecondBattle = true;
    }

    // °á½ÂÀü
    private void FinalBattle()
    {

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

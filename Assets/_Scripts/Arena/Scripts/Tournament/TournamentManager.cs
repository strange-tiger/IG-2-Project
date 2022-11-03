using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TournamentManager : MonoBehaviour
{
    [Header("���� ������ �������� �����ּ���")]
    [SerializeField] private int _startSecond;

    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _vrUI;

    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    private int _selectGroup;
    public int SelectGroup { get { return _selectGroup; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private void OnEnable()
    {
        _selectGroup = Random.Range(0, _groups.Length);

        _selectGroup = 2;

        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
        }

        Invoke("GameStart", _startSecond);
    }
    
    private void GameStart()
    {
        _groups[_selectGroup].SetActive(true);

        _vrUI.SetActive(false);
    }

    private void OnDisable()
    {
        _groups[_selectGroup].SetActive(false);
        _selectGroup -= _selectGroup;
    }
}

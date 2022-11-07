using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TournamentManager : MonoBehaviourPun
{
    [Header("���� ������ �������� �����ּ���")]
    [SerializeField] private int _startSecond;

    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _vrUI;

    [Header("�����")]
    [SerializeField] private ReStartTournament _reStart;

    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    private int _selectGroup;
    public int SelectGroup { get { return _selectGroup; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�� �������Դϴ�");
            _selectGroup = Random.Range(0, _groups.Length);

            // _selectGroup = 2;

            if (_vrUI.activeSelf == false)
            {
                _vrUI.SetActive(true);
            }

            Invoke("GameStart", _startSecond);

            _reStart._startBattle.RemoveListener(GameStartEvent);
            _reStart._startBattle.AddListener(GameStartEvent);
        }
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

        _reStart._startBattle.RemoveListener(GameStartEvent);
    }

    private void GameStartEvent()
    {
        _selectGroup = Random.Range(0, _groups.Length);

        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
        }

        Invoke("GameStart", _startSecond);
    }
}

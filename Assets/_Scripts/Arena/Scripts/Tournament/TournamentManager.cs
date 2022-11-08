using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class TournamentManager : MonoBehaviourPun
{
    [Header("몇초 후 시작할지 적어주세요")]
    [SerializeField] private int _startSecond;

    [Header("배팅 UI를 넣어주세요")]
    [SerializeField] private GameObject _vrUI;

    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    [Header("재시작할 시간초를 넣어주세요")]
    [SerializeField] private float _reStartTime;

    // private UnityEvent _startBattle = new UnityEvent();

    private Action _gameStartActionEvent;

    private int _selectGroupNum;
    public int SelectGroupNum { get { return _selectGroupNum; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private float _curTime;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _gameStartActionEvent = GameStartEvent;

            GameStartEvent();
        }
        Debug.Log("OnEnable");
    }

    private void Update()
    {
        if (_vrUI.activeSelf == false)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _reStartTime)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    _gameStartActionEvent?.Invoke();
                }

                _curTime -= _curTime;
            }
        }
    }

    private void GameStartEvent()
    {
        _selectGroupNum = UnityEngine.Random.Range(0, _groups.Length);

        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
        }

        StartCoroutine(GameStart());
    }

    [PunRPC]
    public void ClientsMustDo(int num)
    {
        Debug.Log("클라 GameStart");

        _vrUI.SetActive(false);
        _groups[num].SetActive(true);
    }

    [PunRPC]
    public void ClientsMustDoEnd(int num)
    {
        _groups[num].SetActive(false);
        num -= num;
    }

    private void OnDisable()
    {
        photonView.RPC("ClientsMustDoEnd", RpcTarget.Others, _selectGroupNum);

        if (PhotonNetwork.IsMasterClient)
        {
            _groups[_selectGroupNum].SetActive(false);
            _selectGroupNum -= _selectGroupNum;
        }
    }

    IEnumerator GameStart()
    {
        while (true)
        {
            yield return new WaitForSeconds(_startSecond);

            photonView.RPC("ClientsMustDo", RpcTarget.Others, _selectGroupNum);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("마스터 GameStart");
                _groups[_selectGroupNum].SetActive(true);
                _vrUI.SetActive(false);
            }

            yield return null;
        }
    }
}

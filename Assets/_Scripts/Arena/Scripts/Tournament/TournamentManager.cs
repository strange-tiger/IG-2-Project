using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class TournamentManager : MonoBehaviourPun
{
    [Header("���� �� �������� �����ּ���")]
    [SerializeField] private int _startSecond;

    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _vrUI;

    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    [Header("������� �ð��ʸ� �־��ּ���")]
    [SerializeField] private float _reStartTime;

    // private UnityEvent _startBattle = new UnityEvent();

    //private Action _gameStartActionEvent;

    private int _selectGroupNum;
    public int SelectGroupNum { get { return _selectGroupNum; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private float _curTime;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameStartEvent();
        }
        Debug.Log("OnEnable");
    }

    private void Update()
    {
        if (_vrUI.activeSelf == false && PhotonNetwork.IsMasterClient)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _reStartTime)
            {
                GameStartEvent();
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

        photonView.RPC("ClientsSetUI", RpcTarget.Others);

        StartCoroutine(GameStart());
    }

    [PunRPC]
    public void ClientsSetUI()
    {
        Debug.Log("��!");
        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
        }
    }

    [PunRPC]
    public void ClientsMustDo(int num)
    {
        Debug.Log("Ŭ�� GameStart");

        _groups[num].SetActive(true);
        _vrUI.SetActive(false);
    }

    [PunRPC]
    public void ClientsMustDoEnd(int num)
    {
        _groups[num].SetActive(false);
        num -= num;
    }

    IEnumerator GameStart()
    {
        WaitForSeconds _startDelay = new WaitForSeconds(_startSecond);

        yield return _startDelay;

        photonView.RPC("ClientsMustDo", RpcTarget.Others, _selectGroupNum);

        if (PhotonNetwork.IsMasterClient)
        {
            _groups[_selectGroupNum].SetActive(true);
            _vrUI.SetActive(false);
        }

        yield break;
    }
}

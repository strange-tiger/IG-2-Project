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

    //private Action _gameStartActionEvent;
    
    private int _selectGroupNum;
    public int SelectGroupNum { get { return _selectGroupNum; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private float _curTime;

    private void Awake()
    {
        Debug.Log($"Awake 동작, 마스터 : {PhotonNetwork.IsMasterClient}, _vrUI의 상태 : {_vrUI.activeSelf}");
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameStartEvent();
        }
        Debug.Log($"Start 동작, 마스터 : {PhotonNetwork.IsMasterClient}, _vrUI의 상태 : {_vrUI.activeSelf}");
    }

    private void Update()
    {
        Debug.Log($"Update 동작, 마스터 : {PhotonNetwork.IsMasterClient}, _vrUI의 상태 : {_vrUI.activeSelf}");

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

        photonView.RPC("ClientsSetUI", RpcTarget.All);

        Debug.Log("GameStartEvent 동작");

        StartCoroutine(GameStart());
    }

    [PunRPC]
    public void ClientsSetUI()
    {
        Debug.Log("PunRPC RpcTarget.All ClientsSetUI()");
        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
        }
    }

    [PunRPC]
    public void ClientsMustDo(int num)
    {
        Debug.Log("PunRPC RpcTarget.All ClientsMustDo()");

        _groups[num].SetActive(true);
        _vrUI.SetActive(false);
    }

    IEnumerator GameStart()
    {
        WaitForSeconds _startDelay = new WaitForSeconds(_startSecond);

        yield return _startDelay;

        photonView.RPC("ClientsMustDo", RpcTarget.All, _selectGroupNum);

        PhotonNetwork.SendAllOutgoingCommands();
    }
}

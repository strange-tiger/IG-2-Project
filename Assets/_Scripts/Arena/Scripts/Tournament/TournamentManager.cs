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

    [SerializeField] private float _reStartTime;

    private float _curTime;

    private int _selectGroupNum;
    public int SelectGroupNum { get { return _selectGroupNum; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private void Awake()
    {
        Debug.Log($"Awake ����, ������ : {PhotonNetwork.IsMasterClient}, _vrUI�� ���� : {_vrUI.activeSelf}");
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameStartEvent();
        }
        Debug.Log($"Start ����, ������ : {PhotonNetwork.IsMasterClient}, _vrUI�� ���� : {_vrUI.activeSelf}");
    }
    

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _curTime += Time.deltaTime;

            if (_curTime >= _reStartTime)
            {
                Debug.Log("�ʱ�ȭ");

                GameStartEvent();
                // PhotonNetwork.Destroy(gameObject);
                _curTime -= _curTime;
            }
        }
    }

    private void GameStartEvent()
    {
        _selectGroupNum = UnityEngine.Random.Range(0, _groups.Length);

        photonView.RPC("ClientsSetUI", RpcTarget.All);

        Debug.Log("GameStartEvent ����");

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

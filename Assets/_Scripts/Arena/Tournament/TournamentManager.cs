using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class TournamentManager : MonoBehaviourPun
{
    [Header("���ʵ��� �������� �����ּ���")]
    [SerializeField] private int _startSecond;

    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _vrUI;

    [Header("������ų �׷��� �־��ּ���")]
    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    // � �׷��� �����ų�� �������� ���� ����
    private int _selectGroupNum;
    public int SelectGroupNum { get { return _selectGroupNum; } private set { _selectGroupNum = value; } }


    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetGroupNum();
        }
    }

    /// <summary>
    /// � �׷��� ���۽�ų�� �����ִ� �Լ�
    /// </summary>
    private void SetGroupNum()
    {
        _selectGroupNum = UnityEngine.Random.Range(0, _groups.Length);

        photonView.RPC(nameof(BettingStart), RpcTarget.All, true);

        StartCoroutine(GameStart());
    }

    /// <summary>
    /// ����UI Ű��
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void BettingStart(bool value)
    {
        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(value);
        }
    }

    /// <summary>
    /// ��������ǰ� ��� ����
    /// </summary>
    /// <param name="num"></param>
    [PunRPC]
    public void BettingEnd(int num)
    {
        _groups[num].SetActive(true);
        _vrUI.SetActive(false);
    }

    IEnumerator GameStart()
    {
        WaitForSeconds _startDelay = new WaitForSeconds(_startSecond);

        yield return _startDelay;

        photonView.RPC(nameof(BettingEnd), RpcTarget.All, _selectGroupNum);

        PhotonNetwork.SendAllOutgoingCommands();
    }
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Events;

public class TournamentManager : MonoBehaviourPun
{
    [Header("몇초동안 베팅할지 적어주세요")]
    [SerializeField] private int _startSecond;

    [Header("베팅 UI를 넣어주세요")]
    [SerializeField] private GameObject _vrUI;

    [Header("참가시킬 그룹을 넣어주세요")]
    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    // 어떤 그룹을 진행시킬지 랜덤값이 담기는 변수
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
    /// 어떤 그룹을 시작시킬지 정해주는 함수
    /// </summary>
    private void SetGroupNum()
    {
        _selectGroupNum = UnityEngine.Random.Range(0, _groups.Length);

        photonView.RPC(nameof(BettingStart), RpcTarget.All, true);

        StartCoroutine(GameStart());
    }

    /// <summary>
    /// 베팅UI 키기
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
    /// 베팅종료되고 경기 시작
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class ReStartTournament : MonoBehaviourPun
{
    [Header("베팅 UI를 넣어주세요")]
    [SerializeField] private GameObject _bettingUI;

    [Header("재시작할 시간초를 넣어주세요")]
    [SerializeField] private float _reStartTime;

    public UnityEvent _startBattle = new UnityEvent();

    private float _curTime;

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_bettingUI.activeSelf == false)
            {
                _curTime += Time.deltaTime;
                Debug.Log((int)_curTime);
                if (_curTime >= _reStartTime)
                {
                    _startBattle.Invoke();
                    photonView.RPC("ClientsStartBattle", RpcTarget.Others);
                    _curTime -= _curTime;
                }
            }
        }
    }

    [PunRPC]
    public void ClientsStartBattle()
    {
        _startBattle.Invoke();
    }
}

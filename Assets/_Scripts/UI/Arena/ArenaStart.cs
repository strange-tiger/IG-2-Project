using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class ArenaStart : MonoBehaviourPun
{
    public static UnityEvent OnTournamentStart = new UnityEvent();

    [SerializeField] private Button _StartBattleButton;
    [SerializeField] private GameObject _shutDown;

    [Header("게임 재시작 시간초")]
    [SerializeField] private float _reStartCoolTime;

    [Header("나갈 수 있는 시간")]
    [SerializeField] private float _youCanOutTime;

    // 게임 재시작까지 걸리는 시간
    private float _curStartTime;
    // 나가는 문 활성화까지 걸리는 시간
    private float _curOutTime;

    void Start()
    {
        _StartBattleButton.onClick.RemoveListener(OnClickStartBattle);
        _StartBattleButton.onClick.AddListener(OnClickStartBattle);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_StartBattleButton.interactable == false)
            {
                _curStartTime += Time.deltaTime;
                _curOutTime += Time.deltaTime;

                if (_curStartTime >= _reStartCoolTime)
                {
                    _StartBattleButton.interactable = true;
                    _curStartTime -= _curStartTime;
                }

                if (_curOutTime >= _youCanOutTime)
                {
                    _shutDown.SetActive(true);
                    _curOutTime -= _curOutTime;
                }
            }
        }
    }

    /// <summary>
    /// 클릭 시 경기시작
    /// </summary>
    public void OnClickStartBattle()
    {
        photonView.RPC("StartTournament", RpcTarget.AllBuffered, false, 0f);
    }

    [PunRPC]
    public void StartTournament(bool value, float resetTimer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
            _shutDown.SetActive(value);
            _curOutTime = resetTimer;
            _curStartTime = resetTimer;
            OnTournamentStart.Invoke();
        }
        _StartBattleButton.interactable = value;
    }

    private void OnDisable()
    {
        _StartBattleButton.onClick.RemoveListener(OnClickStartBattle);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ArenaStart : MonoBehaviourPun
{
    [SerializeField] private Button _StartBattleButton;

    [SerializeField] private float _reStartCoolTime;

    private bool _onClick;
    private float _curTime;

    void Start()
    {
        _StartBattleButton.onClick.RemoveListener(OnClickStartBattle);
        _StartBattleButton.onClick.AddListener(OnClickStartBattle);
    }

    private void Update()
    {
        if (_StartBattleButton.interactable == false)
        {
            _curTime += Time.deltaTime;

            if (_curTime >= _reStartCoolTime)
            {
                _StartBattleButton.interactable = true;
                _curTime -= _curTime;
            }
        }
    }

    public void OnClickStartBattle()
    {
        photonView.RPC("StartTournament", RpcTarget.All);
        _StartBattleButton.interactable = false;
    }

    [PunRPC]
    public void StartTournament()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
        }
    }

    private void OnDisable()
    {
        _StartBattleButton.onClick.RemoveListener(OnClickStartBattle);
    }

}

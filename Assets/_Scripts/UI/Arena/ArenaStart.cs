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

        if (Input.GetKeyDown(KeyCode.G))
        {
            OnClickStartBattle();
        }
    }

    public void OnClickStartBattle()
    {
        photonView.RPC("StartTournament", RpcTarget.All);
     
    }

    [PunRPC]
    public void StartTournament()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
            _StartBattleButton.interactable = false;
            OnTournamentStart.Invoke();
        }
    }

    private void OnDisable()
    {
        _StartBattleButton.onClick.RemoveListener(OnClickStartBattle);
    }

}

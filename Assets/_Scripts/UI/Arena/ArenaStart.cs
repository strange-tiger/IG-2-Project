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

    [Header("���� ����� �ð���")]
    [SerializeField] private float _reStartCoolTime;

    [Header("���� �� �ִ� �ð�")]
    [SerializeField] private float _youCanOutTime;

    // ���� ����۱��� �ɸ��� �ð�
    private float _curStartTime;
    // ������ �� Ȱ��ȭ���� �ɸ��� �ð�
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
    /// Ŭ�� �� ������
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

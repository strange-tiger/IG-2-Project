using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TournamentManager : MonoBehaviourPun
{
    [Header("���� �� �������� �����ּ���")]
    [SerializeField] private int _startSecond;

    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _vrUI;

    [Header("�����")]
    [SerializeField] private ReStartTournament _reStart;

    [SerializeField] private GameObject[] _groups;
    public GameObject[] Groups { get { return _groups; } }

    private int _selectGroup;
    public int SelectGroup { get { return _selectGroup; } }

    private int _finalWinnerIndex;
    public int FinalWinnerIndex { get { return _finalWinnerIndex; } private set { _finalWinnerIndex = value; } }

    private float _curTime;

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _selectGroup = Random.Range(0, _groups.Length);
            Debug.Log("�� �������Դϴ�");

            if (_vrUI.activeSelf == false)
            {
                _vrUI.SetActive(true);
            }

            Invoke("GameStart", _startSecond);

            _reStart._startBattle.RemoveListener(GameStartEvent);
            _reStart._startBattle.AddListener(GameStartEvent);
        }
        Debug.Log("����");
    }

    private void GameStart()
    {
        photonView.RPC("ClientsMustDo", RpcTarget.Others, _selectGroup);

        if (PhotonNetwork.IsMasterClient)
        {
            _groups[_selectGroup].SetActive(true);
            _vrUI.SetActive(false);

            Debug.LogError(_selectGroup);
        }
    }

    [PunRPC]
    public void ClientsMustDo(int num)
    {
        Debug.Log("����ȭ ��ŸƮ");

        _vrUI.SetActive(false);
        _groups[num].SetActive(true);

        Debug.LogError(num);
    }

    private void OnDisable()
    {
        photonView.RPC("ClientsMustDoEnd", RpcTarget.Others, _selectGroup);

        if (PhotonNetwork.IsMasterClient)
        {
            _groups[_selectGroup].SetActive(false);
            _selectGroup -= _selectGroup;
        }
        _reStart._startBattle.RemoveListener(GameStartEvent);
    }

    [PunRPC]
    public void ClientsMustDoEnd(int num)
    {
        _groups[num].SetActive(false);
        num -= num;
    }

    private void GameStartEvent()
    {
        _selectGroup = Random.Range(0, _groups.Length);

        if (_vrUI.activeSelf == false)
        {
            _vrUI.SetActive(true);
            photonView.RPC("ClientsGameStartEvent", RpcTarget.Others);
        }

        Invoke("GameStart", _startSecond);
    }

    [PunRPC]
    public void ClientsGameStartEvent()
    {
        _vrUI.SetActive(true);
    }
}

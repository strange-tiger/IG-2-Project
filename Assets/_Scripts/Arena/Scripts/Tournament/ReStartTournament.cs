using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ReStartTournament : MonoBehaviour
{
    [Header("토너먼트 매니저를 넣어주세요")]
    [SerializeField] private GameObject _tournamentManager;

    [Header("재시작할 시간초를 넣어주세요")]
    [SerializeField] private float _reStartTime;

    private float _curTime;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_tournamentManager.activeSelf == false)
            {
                _curTime += Time.deltaTime;

                if (_curTime >= _reStartTime)
                {
                    _curTime -= _curTime;
                    _tournamentManager.SetActive(true);
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class ReStartTournament : MonoBehaviour
{
    [Header("���� UI�� �־��ּ���")]
    [SerializeField] private GameObject _bettingUI;

    [Header("������� �ð��ʸ� �־��ּ���")]
    [SerializeField] private float _reStartTime;

    public UnityEvent _startBattle = new UnityEvent();

    private int a; 
    private float _curTime;

    private void Update()
    {
        //if (PhotonNetwork.IsMasterClient)
        {
            if (_bettingUI.activeSelf == false)
            {
                _curTime += Time.deltaTime;

                if (_curTime >= _reStartTime)
                {
                    _startBattle.Invoke();
                    _curTime -= _curTime;
                }
            }
        }
    }
    
}

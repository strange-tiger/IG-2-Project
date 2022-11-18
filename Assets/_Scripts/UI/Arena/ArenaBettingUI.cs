using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class ArenaBettingUI : MonoBehaviourPun
{
    
    [SerializeField] private TournamentManager _tournamentManager;
    
    [SerializeField] private TextMeshProUGUI _groupNameText;

    [Header("�����ڵ� �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    private int _groupNum;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _groupNum = _tournamentManager.SelectGroupNum;

            photonView.RPC("BettingForChampionSetting", RpcTarget.All, _groupNum);
        }
    }

    [PunRPC]
    public void BettingForChampionSetting(int num)
    {
        _groupNameText.text = _tournamentManager.Groups[num].name;

        for (int i = 0; i < 4; ++i)
        {
            _championNameText[i].text = _tournamentManager.Groups[num].transform.GetChild(i).name;
        }
    }
}

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

    [Header("참가자들 텍스트")]
    [SerializeField] private TextMeshProUGUI[] _championNameText;

    void Start()
    {
         photonView.RPC("BettingForChampionSetting", RpcTarget.All);
    }

    [PunRPC]
    public void BettingForChampionSetting()
    {
        _groupNameText.text = _tournamentManager.Groups[_tournamentManager.SelectGroupNum].name;

        for (int i = 0; i < 4; ++i)
        {
            _championNameText[i].text = _tournamentManager.Groups[_tournamentManager.SelectGroupNum].transform.GetChild(i).name;
        }
    }
}

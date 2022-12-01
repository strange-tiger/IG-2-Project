using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;
using TMPro;
using Photon.Pun;

public class DistributeUI : MonoBehaviourPun
{

    private void OnEnable()
    {
        BettingManager.OnBettingWinOrLose.RemoveListener(WinOrLoseBetting);
        BettingManager.OnBettingWinOrLose.AddListener(WinOrLoseBetting);

        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
        MySqlSetting.OnBettingDraw.AddListener(DrawBetting);
    }

    private void WinOrLoseBetting(Dictionary<string,int> winnerListDictionary)
    {
        Debug.Log("Betting DB Event OutSide");
        Debug.Log(PhotonNetwork.NickName);

        if (winnerListDictionary.ContainsKey(PhotonNetwork.NickName))
        {
            MenuUIManager.Instance.ShowConfirmPanel($"베팅에 성공하여 {winnerListDictionary[PhotonNetwork.NickName]} Gold를 획득하셨습니다.");

            Debug.Log("WinInSide");
        }
        else
        {
            MenuUIManager.Instance.ShowConfirmPanel("베팅에 실패하여 골드를 잃었습니다.");

            Debug.Log("LoseInSide");
        }
    }

    private void DrawBetting()
    {
        MenuUIManager.Instance.ShowConfirmPanel("무승부입니다. 모든 골드를 다시 돌려드리겠습니다.");
    }

    private void OnDisable()
    {
        BettingManager.OnBettingWinOrLose.RemoveListener(WinOrLoseBetting);
        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
    }
}

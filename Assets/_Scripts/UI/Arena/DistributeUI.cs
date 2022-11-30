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
        MySqlSetting.OnBettingWinOrLose.RemoveListener(WinOrLoseBetting);
        MySqlSetting.OnBettingWinOrLose.AddListener(WinOrLoseBetting);

        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
        MySqlSetting.OnBettingDraw.AddListener(DrawBetting);
    }

    private void WinOrLoseBetting(Dictionary<string,int> winnerListDictionary)
    {
        Debug.Log("Betting DB Event OutSide");

        if (winnerListDictionary.ContainsKey(PhotonNetwork.NickName))
        {
            MenuUIManager.Instance.ShowConfirmPanel($"���ÿ� �����Ͽ� {winnerListDictionary[PhotonNetwork.NickName]} Gold�� ȹ���ϼ̽��ϴ�.");

            Debug.Log("WinInSide");
        }
        else
        {
            MenuUIManager.Instance.ShowConfirmPanel("���ÿ� �����Ͽ� ��带 �Ҿ����ϴ�.");

            Debug.Log("LoseInSide");
        }
    }

    private void DrawBetting()
    {
        MenuUIManager.Instance.ShowConfirmPanel("���º��Դϴ�. ��� ��带 �ٽ� �����帮�ڽ��ϴ�.");
    }

    private void OnDisable()
    {
        MySqlSetting.OnBettingWinOrLose.RemoveListener(WinOrLoseBetting);
        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;
using TMPro;
using Photon.Pun;

public class DistributeUI : MonoBehaviourPun
{
    private bool _isBettingWin;

    private void OnEnable()
    {

        MySqlSetting.OnBettingWin.RemoveListener(WinBetting);
        MySqlSetting.OnBettingWin.AddListener(WinBetting);

        MySqlSetting.OnBettingLose.RemoveListener(LoseBetting);
        MySqlSetting.OnBettingLose.AddListener(LoseBetting);

        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
        MySqlSetting.OnBettingDraw.AddListener(DrawBetting);

    }

    private void WinBetting(string nickname, int gold)
    {
        Debug.Log("WinOutSide");
        if (PhotonNetwork.NickName == nickname)
        {
            MenuUIManager.Instance.ShowConfirmPanel($"베팅에 성공하여 {gold} Gold를 획득하셨습니다.");

            Debug.Log("WinInSide");
            _isBettingWin = true;
        }
    }

    private void LoseBetting()
    {
        Debug.Log("LoseOutSide");
        if (_isBettingWin == false)
        {
            Debug.Log("LoseInSide");
            MenuUIManager.Instance.ShowConfirmPanel("베팅에 실패하여 골드를 잃었습니다.");
        }

        _isBettingWin = false;
    }

    private void DrawBetting()
    {
        MenuUIManager.Instance.ShowConfirmPanel("무승부입니다. 모든 골드를 다시 돌려드리겠습니다.");
    }

    private void OnDisable()
    {

        MySqlSetting.OnBettingWin.RemoveListener(WinBetting);
        MySqlSetting.OnBettingLose.RemoveListener(LoseBetting);
        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);

    }
}

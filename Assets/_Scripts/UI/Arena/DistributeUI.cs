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
        if(photonView.IsMine)
        {
            MySqlSetting.OnBettingWin.RemoveListener(WinBetting);
            MySqlSetting.OnBettingWin.AddListener(WinBetting);

            MySqlSetting.OnBettingLose.RemoveListener(LoseBetting);
            MySqlSetting.OnBettingLose.AddListener(LoseBetting);
        }
    }

    private void WinBetting(string nickname, int gold)
    {
        if (PhotonNetwork.NickName == nickname)
        {
            MenuUIManager.Instance.ShowConfirmPanel($"베팅에 성공하여 {gold}를 획득하셨습니다.");

            _isBettingWin = true;
        }
    }

    private void LoseBetting()
    {
        if (_isBettingWin == false)
        {
            MenuUIManager.Instance.ShowConfirmPanel("베팅에 실패하여 골드를 잃었습니다.");
        }

        _isBettingWin = false;
    }

    private void OnDisable()
    {
        if (photonView.IsMine)
        {
            MySqlSetting.OnBettingWin.RemoveListener(WinBetting);
            MySqlSetting.OnBettingLose.RemoveListener(LoseBetting);
        }
    }
}

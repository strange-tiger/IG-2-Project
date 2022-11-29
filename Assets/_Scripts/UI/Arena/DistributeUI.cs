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
            MenuUIManager.Instance.ShowConfirmPanel($"���ÿ� �����Ͽ� {gold}�� ȹ���ϼ̽��ϴ�.");

            _isBettingWin = true;
        }
    }

    private void LoseBetting()
    {
        if (_isBettingWin == false)
        {
            MenuUIManager.Instance.ShowConfirmPanel("���ÿ� �����Ͽ� ��带 �Ҿ����ϴ�.");
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

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
            MenuUIManager.Instance.ShowConfirmPanel($"���ÿ� �����Ͽ� {gold} Gold�� ȹ���ϼ̽��ϴ�.");

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
            MenuUIManager.Instance.ShowConfirmPanel("���ÿ� �����Ͽ� ��带 �Ҿ����ϴ�.");
        }

        _isBettingWin = false;
    }

    private void DrawBetting()
    {
        MenuUIManager.Instance.ShowConfirmPanel("���º��Դϴ�. ��� ��带 �ٽ� �����帮�ڽ��ϴ�.");
    }

    private void OnDisable()
    {

        MySqlSetting.OnBettingWin.RemoveListener(WinBetting);
        MySqlSetting.OnBettingLose.RemoveListener(LoseBetting);
        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);

    }
}

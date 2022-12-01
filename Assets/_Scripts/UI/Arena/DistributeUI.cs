using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asset.MySql;
using TMPro;
using Photon.Pun;

// 투기장에서 경기가 끝난 뒤, 베팅 결과에 따른 UI를 출력하는 스크립트.
public class DistributeUI : MonoBehaviourPun
{

    private void OnEnable()
    {
        // BettingManager에서 받은 이벤트
        BettingManager.OnBettingWinOrLose.RemoveListener(WinOrLoseBetting);
        BettingManager.OnBettingWinOrLose.AddListener(WinOrLoseBetting);

        // 무승부라면 DBUtil에서 바로 이벤트를 Invoke함.
        MySqlSetting.OnBettingDraw.RemoveListener(DrawBetting);
        MySqlSetting.OnBettingDraw.AddListener(DrawBetting);
    }

    // 이벤트를 통해 플레이어의 닉네임과 획득 골드를 담은 Dictionary를 매개 변수를 받아
    private void WinOrLoseBetting(Dictionary<string,int> winnerListDictionary)
    {
        // Dictionary에 플레이어의 닉네임이 있다면
        if (winnerListDictionary.ContainsKey(PhotonNetwork.NickName))
        {
            // 베팅 성공 텍스트와 닉네임 키에 맞는 획득 골드를 출력
            MenuUIManager.Instance.ShowConfirmPanel($"베팅에 성공하여 {winnerListDictionary[PhotonNetwork.NickName]} Gold를 획득하셨습니다.");
        }
        else
        {
            // 플레이어의 닉네임이 존재하지 않는다면 베팅 실패 텍스트를 출력
            MenuUIManager.Instance.ShowConfirmPanel("베팅에 실패하여 골드를 잃었습니다.");
        }
    }

    // 무승부 일 때, 무승부 텍스트를 출력.
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

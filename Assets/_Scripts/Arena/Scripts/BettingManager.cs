using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingManager : MonoBehaviour
{
    public Dictionary<string, double> BettingOneList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingTwoList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingThreeList = new Dictionary<string, double>();
    public Dictionary<string, double> BettingFourList = new Dictionary<string, double>();

    public double BetAmount;
    public double[] BetRates;
    public double[] ChampionBetAmounts;

    //분배(DB에 등록), 베팅 시작, 종료 알림, 금액 수정시, Dictionary에서 값을 찾아서 반환.

    private void Awake()
    {
        
    }

    private void DistributeGold()
    {

    }

    private void BettingStart()
    {

    }

  
    private void BettingEnd()
    {
        ResetAllBetting();
    }


    private void ResetAllBetting()
    {
        BetAmount = 0;
        BettingOneList.Clear();
        BettingTwoList.Clear();
        BettingThreeList.Clear();
        BettingFourList.Clear();

        for(int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
        }
    }
}

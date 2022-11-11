using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Asset.MySql;
public class BettingManager : MonoBehaviour
{

    public double BetAmount;
    public double[] BetRates;
    public double[] ChampionBetAmounts;

    public UnityEvent OnBettingStart = new UnityEvent();
    public UnityEvent OnBettingEnd = new UnityEvent();

    private bool _isBettingStart;
    private int[] _startTime = { 55, 60, 25, 30 };
    private bool _isDraw;

    [SerializeField] GroupManager _groupManager;

    private void Start()
    {
        if ((DateTime.Now.Minute >= _startTime[0] && DateTime.Now.Minute < _startTime[1]) || (DateTime.Now.Minute >= _startTime[2] && DateTime.Now.Minute < _startTime[3]))
        {
            BettingStart();
        }
    }
    private void Update()
    {
        if(!_isBettingStart)
        {
            if(DateTime.Now.Minute == _startTime[0] || DateTime.Now.Minute == _startTime[1])
            {
                BettingStart();
            }
        }
        else
        {
            if ((DateTime.Now.Minute == 59 || DateTime.Now.Minute == 29) && DateTime.Now.Second >= 30)
            {
                BettingEnd();
            }
        }

    }
    private void DistributeGold()
    {
        MySqlSetting.DistributeBet(_groupManager.WinnerIndex, BetAmount, ChampionBetAmounts[_groupManager.WinnerIndex], _isDraw);

        ResetAllBetting();
    }

    private void BettingStart()
    {
        _isBettingStart = true;
        OnBettingStart.Invoke();
    }

    private void BettingEnd()
    {
        _isBettingStart = false;
        OnBettingEnd.Invoke();
    }


    private void ResetAllBetting()
    {
        BetAmount = 0;

        for(int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
        }
    }
}

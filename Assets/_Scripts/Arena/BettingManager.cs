using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;
public class BettingManager : MonoBehaviourPunCallbacks
{

    public double BetAmount;
    public double[] BetRates;
    public double[] ChampionBetAmounts;
    public int WinnerIndex;

    public UnityEvent OnBettingStart = new UnityEvent();
    public UnityEvent OnBettingEnd = new UnityEvent();

    private bool _isBettingStart;
    private int[] _startTime = { 55, 60, 25, 30 };
    private bool _isDraw;
    private int _playGroupNum;

    private List<double> _bettingAmountList = new List<double>();

    [SerializeField] private BettingUI _bettingUI;
    [SerializeField] private TournamentManager _tournamentManager;
    [SerializeField] private GroupManager[] _groupManager;

    private void Start()
    {
        //if ((DateTime.Now.Minute >= _startTime[0] && DateTime.Now.Minute < _startTime[1]) || (DateTime.Now.Minute >= _startTime[2] && DateTime.Now.Minute < _startTime[3]))
        //{
        //    BettingStart();
        //}

        UpdateBettingAmount();
    }

    public override void OnEnable()
    {
        base.OnEnable();


        _playGroupNum = _tournamentManager.SelectGroupNum;

        _bettingUI.OnBetChampion.RemoveListener(CallBetAmountUpdate);
        _bettingUI.OnBetChampion.AddListener(CallBetAmountUpdate);

        _bettingUI.OnBetCancelChampion.RemoveListener(CallBetCancelAmountUpdate);
        _bettingUI.OnBetCancelChampion.AddListener(CallBetCancelAmountUpdate);

        _groupManager[_playGroupNum]._finishTournament.RemoveListener(BettingEnd);
        _groupManager[_playGroupNum]._finishTournament.AddListener(BettingEnd);

        BettingStart();
    }

    private void Update()
    {
        //if(!_isBettingStart)
        //{
        //    //if(DateTime.Now.Minute == _startTime[0] || DateTime.Now.Minute == _startTime[1])
        //    //{
        //    //    BettingStart();
        //    //}

        //}
        //else
        //{
        //    //if ((DateTime.Now.Minute == 59 || DateTime.Now.Minute == 29) && DateTime.Now.Second >= 30)
        //    //{
        //    //    BettingEnd();
        //    //}

        //}


    }

    private void DistributeGold()
    {
        UpdateBettingAmount();

        MySqlSetting.DistributeBet(WinnerIndex, BetAmount, ChampionBetAmounts[WinnerIndex], _isDraw);

        ResetAllBetting();
    }

    private void BettingStart()
    {
        _isBettingStart = true;
        OnBettingStart.Invoke();
    }

    private void BettingEnd()
    {
        DistributeGold();
        _isBettingStart = false;
        OnBettingEnd.Invoke();
    }

    private void CallBetAmountUpdate(int index, double bettingGold)
    {
        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        photonView.RPC("BetAmountUpdate", RpcTarget.All, index, bettingGold);
        
    }

    private void CallBetCancelAmountUpdate(int index, double cancelGold)
    {
        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        photonView.RPC("BetCancelAmountUpdate", RpcTarget.All, index, cancelGold);
    }



    [PunRPC]
    public void BetAmountUpdate(int index, double bettingGold)
    {
        BetAmount += bettingGold;

        ChampionBetAmounts[index] += bettingGold;


        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        for (int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = (ChampionBetAmounts[i] / BetAmount) * 100;
            _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
        }

        
    }

    [PunRPC]
    public void BetCancelAmountUpdate(int index, double cancelGold)
    {

        BetAmount -= cancelGold;

        ChampionBetAmounts[index] -= cancelGold;

        for (int i = 0; i < BetRates.Length; ++i)
        {
            if ((ChampionBetAmounts[i] != 0))
            {
                BetRates[i] = (ChampionBetAmounts[i] / BetAmount) * 100;
                _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
            }
            else
            {
                BetRates[i] = 0;
                _bettingUI.BetRateText[i].text = "0";
            }
        }



    }
    private void ResetAllBetting()
    {
        BetAmount = 0;

        for(int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
            MySqlSetting.UpdateBettingAmountDB(i, 0, 0);
        }
    }

    public void UpdateBettingAmount()
    {
        _bettingAmountList = MySqlSetting.CheckBettingAmount();

        BetAmount = _bettingAmountList[0];

        for(int i = 0; i < ChampionBetAmounts.Length; ++i)
        {
            ChampionBetAmounts[i] = _bettingAmountList[i + 1];

            if ((ChampionBetAmounts[i] != 0))
            {
                BetRates[i] = (ChampionBetAmounts[i] / BetAmount) * 100;
                _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
            }
            else
            {
                BetRates[i] = 0;
                _bettingUI.BetRateText[i].text = "0";
            }
        }
    }


    public override void OnDisable()
    {
        base.OnEnable();
        _groupManager[_playGroupNum]._finishTournament.RemoveListener(BettingEnd);
        _bettingUI.OnBetChampion.RemoveListener(CallBetAmountUpdate);
        _bettingUI.OnBetCancelChampion.RemoveListener(CallBetCancelAmountUpdate);

    }
}
public enum ChampionNumber
{
    OneAmount,
    TwoAmount,
    ThreeAmount,
    FourAmount
};
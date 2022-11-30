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
    public UnityEvent OnBettingStart = new UnityEvent();
    public UnityEvent OnBettingEnd = new UnityEvent();

    [SerializeField] private BettingUI _bettingUI;
    [SerializeField] private TournamentManager _tournamentManager;
    [SerializeField] private GroupManager[] _groupManager;

    private List<int> _bettingAmountList = new List<int>();

    public int BetAmount;
    public int[] ChampionBetAmounts;
    public double[] BetRates;

    public int WinnerIndex;

    private bool _isBettingStart;
    private int[] _startTime = { 55, 60, 25, 30 };

    private bool _isDraw;
    private int _playGroupNum;

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

        _isDraw = false;

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


    private void BettingStart()
    {
        _isBettingStart = true;
        OnBettingStart.Invoke();
    }

    public void UpdateBettingAmount()
    {
        _bettingAmountList = MySqlSetting.CheckBettingAmount();

        BetAmount = _bettingAmountList[0];

        for (int i = 0; i < ChampionBetAmounts.Length; ++i)
        {
            ChampionBetAmounts[i] = _bettingAmountList[i + 1];

            if ((ChampionBetAmounts[i] != 0))
            {
                BetRates[i] = (double.Parse(ChampionBetAmounts[i].ToString()) / double.Parse(BetAmount.ToString())) * 100;
                _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
            }
            else
            {
                BetRates[i] = 0;
                _bettingUI.BetRateText[i].text = "0";
            }
        }
    }

    private void CallBetAmountUpdate(int index, int bettingGold)
    {
        photonView.RPC("BetAmountUpdate", RpcTarget.All, index, bettingGold);
    }

    [PunRPC]
    public void BetAmountUpdate(int index, int bettingGold)
    {
        BetAmount += bettingGold;

        ChampionBetAmounts[index] += bettingGold;

        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        for (int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = (double.Parse(ChampionBetAmounts[i].ToString()) / double.Parse(BetAmount.ToString())) * 100;
            _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
        }
    }
    private void CallBetCancelAmountUpdate(int index, int cancelGold)
    {
        photonView.RPC("BetCancelAmountUpdate", RpcTarget.All, index, cancelGold);
    }

    [PunRPC]
    public void BetCancelAmountUpdate(int index, int cancelGold)
    {

        BetAmount -= cancelGold;

        ChampionBetAmounts[index] -= cancelGold;

        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        for (int i = 0; i < BetRates.Length; ++i)
        {
            if ((ChampionBetAmounts[i] != 0))
            {
                BetRates[i] = (double.Parse(ChampionBetAmounts[i].ToString()) / double.Parse(BetAmount.ToString())) * 100;
                _bettingUI.BetRateText[i].text = $"{Math.Round(BetRates[i])}";
            }
            else
            {
                BetRates[i] = 0;
                _bettingUI.BetRateText[i].text = "0";
            }
        }
    }
    private void DistributeGold()
    {
        UpdateBettingAmount();

        photonView.RPC("DistributeGoldinDB", RpcTarget.All);

        ResetAllBetting();
    }

    [PunRPC]
    public void DistributeGoldinDB()
    {
        MySqlSetting.DistributeBet(WinnerIndex, BetAmount, ChampionBetAmounts[WinnerIndex], _isDraw);
    }

    private void ResetAllBetting()
    {
        BetAmount = 0;

        for (int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
            MySqlSetting.UpdateBettingAmountDB(i, 0, 0);
        }
    }

    private void BettingEnd()
    {
        DistributeGold();
        _isBettingStart = false;
        OnBettingEnd.Invoke();
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
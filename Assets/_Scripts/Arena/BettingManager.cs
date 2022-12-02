using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;


/*
 * 베팅을 연산하여 베팅율을 보여주고, 경기가 종료되면 DB를 참고하여 골드를 분배함.
 * BettingDB => 각 플레이어의 닉네임, 베팅 금액, 베팅한 챔피언의 인덱스를 저장함.
 * BettingAmountDB => 각 챔피언에게 베팅된 총 금액과 전체 베팅액을 저장함.
 * 
 * 베팅 시작 
 * 기획 => 25분, 55분으로 투기장 경기가 시작하기 5분전에 시작하고, 경기 시작 1분전에 베팅이 종료됨.
 * 현재(테스트) => 투기장의 GameStart 버튼을 누르면 베팅이 시작되고 약 3분 뒤에 베팅이 종료되고 경기가 시작됨.
 * 
 */
public class BettingManager : MonoBehaviourPunCallbacks
{
    // 베팅 시작과 끝을 BettingUI에 알려주는 이벤트.
    public UnityEvent OnBettingStart = new UnityEvent();
    public UnityEvent OnBettingEnd = new UnityEvent();

    // DBUtil에서 골드를 분배할 때 반환되는 Dictionary를 받아 DistributeUI에 전달, 각각의 플레이어에게 베팅 성공과 획득 골드, 실패를 알리는 UI 를 띄우게 함.
    public static UnityEvent<Dictionary<string, int>> OnBettingWinOrLose = new UnityEvent<Dictionary<string, int>>();

    [Header("Tournament")]
    [SerializeField] private BettingUI _bettingUI;
    [SerializeField] private TournamentManager _tournamentManager;
    [SerializeField] private GroupManager[] _groupManager;

    // 베팅 총 금액을 저장하는 리스트.
    private List<int> _bettingAmountList = new List<int>();

    // 베팅에 성공한 플레이어의 닉네임과 획득 골드를 받아 저장하는 Dictionary.
    private Dictionary<string, int> _bettingWinnerList = new Dictionary<string, int>();

    // 총 베팅 금액과 챔피언별 총 베팅금액, 베팅율.
    public int BetAmount;
    public int[] ChampionBetAmounts;
    public double[] BetRates;

    // 경기에서 승리한 챔피언의 인덱스.
    public int WinnerIndex;

    // 시간으로 투기장을 시작할 때 사용되는 변수.
    [Obsolete]
    private int[] _startTime = { 55, 60, 25, 30 };

    // 베팅의 시작 여부.
    private bool _isBettingStart;

    // 무승부 여부.
    private bool _isDraw;

    // 현재 경기를 진행할 그룹의 인덱스.
    private int _playGroupNum;

    /// <summary>
    /// 시스템의 시간을 판단하여 베팅을 시작함.
    /// BettingUI의 베팅율 등을 초기화.
    /// </summary>
    private void Start()
    {
        /*
        if ((DateTime.Now.Minute >= _startTime[0] && DateTime.Now.Minute < _startTime[1]) || (DateTime.Now.Minute >= _startTime[2] && DateTime.Now.Minute < _startTime[3]))
        {
            BettingStart();
        }
        */
        UpdateBettingAmount();
    }

    /// <summary>
    /// 경기에 관련된 변수들을 초기화 하고 베팅을 시작함.
    /// </summary>
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

    /// <summary>
    /// 시간을 Update에서 판단하여 정해진 시간이 되면 베팅을 시작함.
    /// </summary>
    private void Update()
    {
        /*
        if (PhotonNetwork.IsMasterClient)
        {
            if (!_isBettingStart)
            {
                if (DateTime.Now.Minute == _startTime[0] || DateTime.Now.Minute == _startTime[1])
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
        */
    }

    /// <summary>
    /// 베팅을 시작함. 시작과 동시에 MasterClient에서는 이전에 저장되어있던 BettingDB, BettingAmountDB를 리셋함.
    /// </summary>
    private void BettingStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ResetAllBetting();
        }

        _isBettingStart = true;

        OnBettingStart.Invoke();
    }

    /// <summary>
    /// 베팅 총 금액을 DB에서 불러와 리스트에 저장하여 베팅율을 업데이트함.
    /// </summary>
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

    /// <summary>
    /// BettingUI에서 베팅을 했을 때, BettingAmountDB를 업데이트하는 RPC를 호출하는 메서드.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="bettingGold"></param>
    private void CallBetAmountUpdate(int index, int bettingGold)
    {
        photonView.RPC("BetAmountUpdate", RpcTarget.All, index, bettingGold);
    }

    /// <summary>
    /// 베팅을 적용하여 BettingAmount를 DB에 업데이트하고, 베팅율을 업데이트함. 
    /// </summary>
    /// <param name="index"> 베팅한 챔피언의 인덱스 </param>
    /// <param name="bettingGold"> 베팅 금액</param>
    [PunRPC]
    public void BetAmountUpdate(int index, int bettingGold)
    {
        // 저장되어있던 베팅 총 금액에 매개변수로 받은 베팅 금액을 더하고
        BetAmount += bettingGold;

        // 베팅한 챔피언의 총 금액도 같은 방식으로 업데이트함.
        ChampionBetAmounts[index] += bettingGold;

        // 연산한 금액을 DB에 저장함.
        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        // 베팅율과 베팅 금액 리스트도 함께 업데이트.
        UpdateBettingAmount();
    }

    /// <summary>
    /// BettingUI에서 베팅을 취소 했을 때, BettingAmountDB를 업데이트하는 RPC를 호출하는 메서드.
    /// </summary>
    /// <param name="index"> 베팅을 취소한 챔피언의 인덱스 </param>
    /// <param name="cancelGold"> 취소한 금액 </param>
    private void CallBetCancelAmountUpdate(int index, int cancelGold)
    {
        photonView.RPC("BetCancelAmountUpdate", RpcTarget.All, index, cancelGold);
    }

    /// <summary>
    ///  베팅 취소를 적용하여 BettingAmount를 DB에 업데이트하고, 베팅율을 업데이트함. 
    /// </summary>
    /// <param name="index">베팅을 취소한 챔피언의 인덱스</param>
    /// <param name="cancelGold">베팅을 취소한 금액</param>
    [PunRPC]
    public void BetCancelAmountUpdate(int index, int cancelGold)
    {

        BetAmount -= cancelGold;

        ChampionBetAmounts[index] -= cancelGold;

        MySqlSetting.UpdateBettingAmountDB(index, BetAmount, ChampionBetAmounts[index]);

        UpdateBettingAmount();
    }

    /// <summary>
    /// 베팅 총 금액을 업데이트하고, 승자 리스트 Dictionary를 받고 골드를 분배함.
    /// 받은 승자 리스트 Dictionary는 모든 플레이어에게 RPC로 전달하여 이벤트를 호출하게 함.
    /// </summary>
    private void DistributeGold()
    {
        UpdateBettingAmount();

        _bettingWinnerList = MySqlSetting.DistributeBet(WinnerIndex, BetAmount, ChampionBetAmounts[WinnerIndex], _isDraw);


        photonView.RPC("DistributeGoldinDB", RpcTarget.All, _bettingWinnerList);
    }

    [PunRPC]
    public void DistributeGoldinDB(Dictionary<string,int> winnerList)
    {
        // BettingDB에 베팅을 한 정보가 있으면
        if (MySqlSetting.HasValue(Asset.EbettingdbColumns.Nickname, PhotonNetwork.NickName))
        {
            // 이벤트를 호출함.
            OnBettingWinOrLose.Invoke(winnerList);
        }
    }

    /// <summary>
    ///  베팅 관련한 변수와 DB를 모두 초기화함.
    /// </summary>
    private void ResetAllBetting()
    {
        BetAmount = 0;

        MySqlSetting.ResetBettingDB();

        for (int i = 0; i < BetRates.Length; ++i)
        {
            BetRates[i] = 0;
            ChampionBetAmounts[i] = 0;
            MySqlSetting.UpdateBettingAmountDB(i, 0, 0);
        }
    }

    /// <summary>
    /// 베팅이 끝남을 알리는 이벤트를 호출하고 골드를 분배함.
    /// </summary>
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
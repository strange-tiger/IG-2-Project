using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingManager : MonoBehaviour
{
    public Dictionary<string, float> BettingOneList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingTwoList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingThreeList = new Dictionary<string, float>();
    public Dictionary<string, float> BettingFourList = new Dictionary<string, float>();

    public float BetAmount;
    public float[] BetRate;
    public float[] ChampionBetAmounts;

    //분배(DB에 등록), 베팅 시작,종료 알림, 금액 수정시, Dictionary에서 값을 찾아서 반환.

    private void Awake()
    {
        ;
    }

    private void DistributeGold()
    {

    }

    private void BettingStart()
    {

    }

    private void BettingEnd()
    {

    }

    private void FindBetting()
    {

    }

    private void ResetAllBetting()
    {
        
    }
}

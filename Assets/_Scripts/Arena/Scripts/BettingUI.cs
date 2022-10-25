using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BettingUI : MonoBehaviour
{
    [Header("Betting Button")]
    [SerializeField] Button _betChampionOneButton;
    [SerializeField] Button _betChampionTwoButton;
    [SerializeField] Button _betChampionThreeButton;
    [SerializeField] Button _betChampionFourButton;

    [Header("Betting InputField")]
    [SerializeField] TMP_InputField[] _betChampionInputField;

    [Header("Betting Rate")]
    [SerializeField] TextMeshProUGUI[] _betRateText;


    [SerializeField] BettingManager _bettingManager;

    private string _playerNickname;
    private void Start()
    {
        _playerNickname = "dd";
    }

    private void OnEnable()
    {
        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionOneButton.onClick.AddListener(BetChampionOne);

        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionTwoButton.onClick.AddListener(BetChampionTwo);

        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionThreeButton.onClick.AddListener(BetChampionThree);

        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
        _betChampionFourButton.onClick.AddListener(BetChampionFour);
    }

    private int BetChampion(int index)
    {
        _bettingManager.BetAmount += float.Parse(_betChampionInputField[index].text);
        _bettingManager.ChampionBetAmounts[index] += float.Parse(_betChampionInputField[index].text);
        Debug.Log("µé¾î°¨");
        for(int i = 0; i < _bettingManager.BetRate.Length; ++i)
        {
             _bettingManager.BetRate[i] = _bettingManager.ChampionBetAmounts[i] / _bettingManager.BetAmount;
            _betRateText[i].text = $"{_bettingManager.BetRate[i] * 100}";
        }
        return index;
    }

    private void BetChampionOne() =>  _bettingManager.BettingOneList.Add(_playerNickname, float.Parse(_betChampionInputField[BetChampion(0)].text));

    private void BetChampionTwo() => _bettingManager.BettingTwoList.Add(_playerNickname, float.Parse(_betChampionInputField[BetChampion(1)].text));

    private void BetChampionThree() => _bettingManager.BettingThreeList.Add(_playerNickname, float.Parse(_betChampionInputField[BetChampion(2)].text));

    private void BetChampionFour() => _bettingManager.BettingFourList.Add(_playerNickname, float.Parse(_betChampionInputField[BetChampion(3)].text));

    private void OnDisable()
    {
        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
    }
}

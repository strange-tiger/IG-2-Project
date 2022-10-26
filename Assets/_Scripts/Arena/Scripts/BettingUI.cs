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

    private bool[] _isBetting = new bool[4];
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

        foreach(TMP_InputField inputfield in _betChampionInputField)
        {
            inputfield.onSelect.AddListener((string temp) =>
                {
                    KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
                }
            );
        }
    }

    private bool BettingExist()
    {
        for(int i = 0; i < _isBetting.Length; ++i)
        {
            if(_isBetting[i])
            {
                return true;
            }
        }
        return false;
    }

    private int BetChampion(int index)
    {
        if(BettingExist())
        {
            //ÆÐ³Î ¿Â
            return -1;
        }
        else
        {
            _isBetting[index] = true;
            _bettingManager.BetAmount += double.Parse(_betChampionInputField[index].text);
            _bettingManager.ChampionBetAmounts[index] += double.Parse(_betChampionInputField[index].text);
            for(int i = 0; i < _bettingManager.BetRates.Length; ++i)
            {
                _bettingManager.BetRates[i] = (_bettingManager.ChampionBetAmounts[i] / _bettingManager.BetAmount) * 100;
                _betRateText[i].text = $"{Math.Round(_bettingManager.BetRates[i])}";
            }

            return index;
        }
    }
    private void BetCancel(int index, double cancelGold)
    {
        
            _isBetting[index] = false;
            _bettingManager.BetAmount -= cancelGold;
            _bettingManager.ChampionBetAmounts[index] -= cancelGold;
            for (int i = 0; i < _bettingManager.BetRates.Length; ++i)
            {
                _bettingManager.BetRates[i] = (_bettingManager.ChampionBetAmounts[i] / _bettingManager.BetAmount) * 100;
                _betRateText[i].text = $"{Math.Round(_bettingManager.BetRates[i])}";
            }
        
       
    }
    private void BetChampionOne()
    {
        if(BetChampion(0) != -1)
        {
            _bettingManager.BettingOneList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(0)].text));
        }
    }

    private void BetChampionTwo()
    {
        if(BetChampion(1) != -1)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(1)].text));
        }
    }

    private void BetChampionThree()
    {
        if (BetChampion(2) != -1)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(2)].text));
        }
    }
    private void BetChampionFour()
    {
        if (BetChampion(3) != -1)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(3)].text));
        }
    }
    private void BetCancelChampionOne()
    {
        if (BettingExist())
        {
            BetCancel(0, _bettingManager.BettingOneList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
    }

    private void BetCancelChampionTwo()
    {
        if (BettingExist())
        {
            BetCancel(1, _bettingManager.BettingTwoList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
    }

    private void BetCancelChampionThree()
    {
        if (BettingExist())
        {
            BetCancel(2, _bettingManager.BettingThreeList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
    }

    private void BetCancelChampionFour()
    {
        if (BettingExist())
        {
            BetCancel(3, _bettingManager.BettingFourList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
    }
    private void OnDisable()
    {
        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
    }
}

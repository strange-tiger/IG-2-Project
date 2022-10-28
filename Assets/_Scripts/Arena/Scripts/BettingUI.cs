using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BettingUI : MonoBehaviour
{

    [Header("Betting Panel")]
    [SerializeField] GameObject _bettingPanel;
    [SerializeField] Button _bettingPanelButton;
    [SerializeField] Button _bettingPanelOffButton;

    [Header("Betting End PopUpPanel")]
    [SerializeField] GameObject _bettingEndPopUpPanel;
    [SerializeField] Button _bettingPopUpPanelOffButton;

    [Header("Betting Button")]
    [SerializeField] Button _betChampionOneButton;
    [SerializeField] Button _betChampionTwoButton;
    [SerializeField] Button _betChampionThreeButton;
    [SerializeField] Button _betChampionFourButton;

    [Header("Betting Cancel Button")]
    [SerializeField] Button _betCancelChampionOneButton;
    [SerializeField] Button _betCancelChampionTwoButton;
    [SerializeField] Button _betCancelChampionThreeButton;
    [SerializeField] Button _betCancelChampionFourButton;

    [Header("Betting PopUp Panel")]
    [SerializeField] GameObject _popUpPanel;
    [SerializeField] TextMeshProUGUI _popUpMessage;
    [SerializeField] Button _popUpOffButton;

    [Header("Betting InputField")]
    [SerializeField] TMP_InputField[] _betChampionInputField;

    [Header("Betting Rate")]
    [SerializeField] TextMeshProUGUI[] _betRateText;


    [SerializeField] BettingManager _bettingManager;

    private bool[] _isBetting = { false, false, false, false };
    private string _playerNickname;

    private void Start()
    {
        _playerNickname = "dd";
    }

    private void OnEnable()
    {
        _bettingManager.OnBettingStart.RemoveListener(BettingStart);
        _bettingManager.OnBettingStart.AddListener(BettingStart);

        _bettingManager.OnBettingEnd.RemoveListener(BettingEnd);
        _bettingManager.OnBettingEnd.AddListener(BettingEnd);

        _bettingPanelButton.onClick.RemoveListener(BettingPanelOn);
        _bettingPanelButton.onClick.AddListener(BettingPanelOn);

        _bettingPanelOffButton.onClick.RemoveListener(BettingPanelOff);
        _bettingPanelOffButton.onClick.AddListener(BettingPanelOff);

        _bettingPopUpPanelOffButton.onClick.RemoveListener(BettingEndPopUpOff);
        _bettingPopUpPanelOffButton.onClick.AddListener(BettingEndPopUpOff);

        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionOneButton.onClick.AddListener(BetChampionOne);

        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionTwoButton.onClick.AddListener(BetChampionTwo);

        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionThreeButton.onClick.AddListener(BetChampionThree);

        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
        _betChampionFourButton.onClick.AddListener(BetChampionFour);

        _betCancelChampionOneButton.onClick.RemoveListener(BetCancelChampionOne);
        _betCancelChampionOneButton.onClick.AddListener(BetCancelChampionOne);

        _betCancelChampionTwoButton.onClick.RemoveListener(BetCancelChampionTwo);
        _betCancelChampionTwoButton.onClick.AddListener(BetCancelChampionTwo);

        _betCancelChampionThreeButton.onClick.RemoveListener(BetCancelChampionThree);
        _betCancelChampionThreeButton.onClick.AddListener(BetCancelChampionThree);

        _betCancelChampionFourButton.onClick.RemoveListener(BetCancelChampionFour);
        _betCancelChampionFourButton.onClick.AddListener(BetCancelChampionFour);

        _popUpOffButton.onClick.RemoveListener(PopUpPanelOff);
        _popUpOffButton.onClick.AddListener(PopUpPanelOff);

        foreach (TMP_InputField inputfield in _betChampionInputField)
        {
            inputfield.onSelect.AddListener((string temp) =>
                {
                    KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
                }
            );
        }
    }

    private void PopUpPanelOff() => _popUpPanel.SetActive(false);

    private void BettingStart() => _bettingPanelButton.gameObject.SetActive(true);
    

    private void BettingEnd()
    {
        _bettingPanelButton.gameObject.SetActive(false);
        _bettingPanel.SetActive(false);
        _bettingEndPopUpPanel.SetActive(true);
    }

    private void BettingEndPopUpOff() => _bettingEndPopUpPanel.SetActive(false);

    private void BettingPanelOn()
    {
        _bettingPanelButton.gameObject.SetActive(false);
        _bettingPanel.SetActive(true);
    }
    private void BettingPanelOff()
    {
        _bettingPanelButton.gameObject.SetActive(true);
        _bettingPanel.SetActive(false);
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
 
        if (!BettingExist())
        {
            _isBetting[index] = true;
            _bettingManager.BetAmount += double.Parse(_betChampionInputField[index].text);
            _bettingManager.ChampionBetAmounts[index] += double.Parse(_betChampionInputField[index].text);
            for(int i = 0; i < _bettingManager.BetRates.Length; ++i)
            {
                _bettingManager.BetRates[i] = (_bettingManager.ChampionBetAmounts[i] / _bettingManager.BetAmount) * 100;
                _betRateText[i].text = $"{Math.Round(_bettingManager.BetRates[i])}";
            }
            _betChampionInputField[index].text = null;
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅이 완료되었습니다.";
            return index;
        }
            return -1;
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
        
        _popUpPanel.SetActive(true);
        _popUpMessage.text = "베팅 취소가 완료되었습니다.";
    }
    private void BetChampionOne()
    {
        if(BettingExist() == false)
        {
            _bettingManager.BettingOneList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(0)].text));
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅을 변경하려면 현재 베팅을 취소해주세요.";
        }
    }

    private void BetChampionTwo()
    {
        if(BettingExist() == false)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(1)].text));
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅을 변경하려면 현재 베팅을 취소해주세요.";
        }
    }

    private void BetChampionThree()
    {
        if (BettingExist() == false)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(2)].text));
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅을 변경하려면 현재 베팅을 취소해주세요.";
        }
    }
    private void BetChampionFour()
    {
        if (BettingExist() == false)
        {
            _bettingManager.BettingTwoList.Add(_playerNickname, double.Parse(_betChampionInputField[BetChampion(3)].text));
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅을 변경하려면 현재 베팅을 취소해주세요.";
        }
    }
    private void BetCancelChampionOne()
    {
        if (BettingExist())
        {
            BetCancel(0, _bettingManager.BettingOneList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }

    private void BetCancelChampionTwo()
    {
        if (BettingExist())
        {
            BetCancel(1, _bettingManager.BettingTwoList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }

    private void BetCancelChampionThree()
    {
        if (BettingExist())
        {
            BetCancel(2, _bettingManager.BettingThreeList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }

    private void BetCancelChampionFour()
    {
        if (BettingExist())
        {
            BetCancel(3, _bettingManager.BettingFourList[_playerNickname]);
            _bettingManager.BettingOneList.Remove(_playerNickname);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }
    private void OnDisable()
    {
        _bettingManager.OnBettingStart.RemoveListener(BettingStart);
        _bettingManager.OnBettingEnd.RemoveListener(BettingEnd);
        _bettingPanelOffButton.onClick.RemoveListener(BettingPanelOff);
        _bettingPanelButton.onClick.RemoveListener(BettingPanelOn);
        _bettingPopUpPanelOffButton.onClick.RemoveListener(BettingEndPopUpOff);
        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
        _betCancelChampionOneButton.onClick.RemoveListener(BetCancelChampionOne);
        _betCancelChampionTwoButton.onClick.RemoveListener(BetCancelChampionTwo);
        _betCancelChampionThreeButton.onClick.RemoveListener(BetCancelChampionThree);
        _betCancelChampionFourButton.onClick.RemoveListener(BetCancelChampionFour);
        _popUpOffButton.onClick.RemoveListener(PopUpPanelOff);
    }
}

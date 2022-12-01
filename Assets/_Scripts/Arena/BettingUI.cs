using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Asset.MySql;

public class BettingUI : MonoBehaviourPun
{

    public UnityEvent<int, int> OnBetChampion = new UnityEvent<int, int>();
    public UnityEvent<int, int> OnBetCancelChampion = new UnityEvent<int, int>();

    [Header("Betting Panel")]
    [SerializeField] private GameObject _bettingPanel;
    [SerializeField] private Button _bettingPanelButton;
    [SerializeField] private Button _bettingPanelOffButton;

    [Header("Betting End PopUpPanel")]
    [SerializeField] private GameObject _bettingEndPopUpPanel;
    [SerializeField] private Button _bettingPopUpPanelOffButton;

    [Header("Betting Button")]
    [SerializeField] private Button _betChampionOneButton;
    [SerializeField] private Button _betChampionTwoButton;
    [SerializeField] private Button _betChampionThreeButton;
    [SerializeField] private Button _betChampionFourButton;

    [Header("Betting Cancel Button")]
    [SerializeField] private Button _betCancelChampionOneButton;
    [SerializeField] private Button _betCancelChampionTwoButton;
    [SerializeField] private Button _betCancelChampionThreeButton;
    [SerializeField] private Button _betCancelChampionFourButton;

    [Header("Betting PopUp Panel")]
    [SerializeField] private GameObject _popUpPanel;
    [SerializeField] private TextMeshProUGUI _popUpMessage;
    [SerializeField] private Button _popUpOffButton;

    [Header("Betting InputField")]
    [SerializeField] private TMP_InputField[] _betChampionInputField;

    [Header("Betting Rate")]
    [SerializeField] public TextMeshProUGUI[] BetRateText;


    [SerializeField] private BettingManager _bettingManager;

    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

    private bool[] _isBetting = { false, false, false, false };

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

        ArenaStart.OnTournamentStart.RemoveListener(BettingUIInit);
        ArenaStart.OnTournamentStart.AddListener(BettingUIInit);

        _playerNetworkings = FindObjectsOfType<BasicPlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        foreach (TMP_InputField inputfield in _betChampionInputField)
        {
            inputfield.onSelect.AddListener((string temp) =>
                {
                    KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
                }
            );
        }
    }

    private void BettingUIInit()
    {
        _bettingManager = FindObjectOfType<BettingManager>();
        _bettingPanelButton.gameObject.SetActive(true);
    }

    private void PopUpPanelOff() => _popUpPanel.SetActive(false);

    private void BettingStart()
    {
        _bettingPanelButton.gameObject.SetActive(true);

        for (int i = 0; i < BetRateText.Length; ++i)
        {
            BetRateText[i].text = null;
            _isBetting[i] = false;
        }
    }

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
        for (int i = 0; i < _isBetting.Length; ++i)
        {
            if (_isBetting[i])
            {
                return true;
            }
        }
        return false;
    }

    private void InputFieldClear()
    {
        for (int i = 0; i < _betChampionInputField.Length; ++i)
        {
            _betChampionInputField[i].text = null;
        }
    }

    [PunRPC]
    public void BetChampionAmount(int index, int bettingGold)
    {
        OnBetChampion.Invoke(index, bettingGold);
    }

    private void BetChampion(int index)
    {

        if (MySqlSetting.CheckHaveGold(_playerNetworking.MyNickname) < int.Parse(_betChampionInputField[index].text))
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅액이 부족합니다.";

            InputFieldClear();

            return;
        }

        MySqlSetting.InsertBetting(_playerNetworking.MyNickname, int.Parse(_betChampionInputField[index].text), index);
        MySqlSetting.UpdateGoldAfterBetting(_playerNetworking.MyNickname, int.Parse(_betChampionInputField[index].text));

        _isBetting[index] = true;

        photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, index, int.Parse(_betChampionInputField[index].text));


        InputFieldClear();

        _popUpPanel.SetActive(true);

        _popUpMessage.text = "베팅이 완료되었습니다.";
    }

    private void BetChampionOne()
    {
        if (BettingExist() == false)
        {
            BetChampion(0);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅을 변경하려면 현재 베팅을 취소해주세요.";
        }
    }

    private void BetChampionTwo()
    {
        if (BettingExist() == false)
        {
            BetChampion(1);
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
            BetChampion(2);
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
            BetChampion(3);
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
            photonView.RPC("BetCancelAmount", RpcTarget.MasterClient, 0, MySqlSetting.CancelBetting(_playerNetworking.MyNickname));
            BetCancel(0);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }

    [PunRPC]
    public void BetCancelAmount(int index, int cancelGold)
    {
        OnBetCancelChampion.Invoke(index, cancelGold);
    }

    private void BetCancel(int index)
    {
        _isBetting[index] = false;

        InputFieldClear();

        _popUpPanel.SetActive(true);


        _popUpMessage.text = "베팅 취소가 완료되었습니다.";
    }
    private void BetCancelChampionTwo()
    {
        if (BettingExist())
        {
            photonView.RPC("BetCancelAmount", RpcTarget.MasterClient, 1, MySqlSetting.CancelBetting(_playerNetworking.MyNickname));
            BetCancel(1);
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
            photonView.RPC("BetCancelAmount", RpcTarget.MasterClient, 2, MySqlSetting.CancelBetting(_playerNetworking.MyNickname));
            BetCancel(2);
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
            photonView.RPC("BetCancelAmount", RpcTarget.MasterClient, 3, MySqlSetting.CancelBetting(_playerNetworking.MyNickname));
            BetCancel(3);
        }
        else
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }
    private void BettingEnd()
    {
        _bettingPanelButton.gameObject.SetActive(false);
        _bettingPanel.SetActive(false);
        _bettingEndPopUpPanel.SetActive(true);
    }

    private void BettingEndPopUpOff() => _bettingEndPopUpPanel.SetActive(false);

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

        ArenaStart.OnTournamentStart.RemoveListener(BettingUIInit);

    }
}

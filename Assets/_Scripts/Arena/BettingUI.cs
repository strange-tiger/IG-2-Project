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

    public UnityEvent<int, double> OnBetChampion = new UnityEvent<int, double>();
    public UnityEvent<int, double> OnBetCancelChampion = new UnityEvent<int, double>();

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
    [SerializeField] public TextMeshProUGUI[] BetRateText;


    [SerializeField] BettingManager _bettingManager;

    private bool[] _isBetting = { false, false, false, false };
    private PlayerNetworking[] _playerNetworkings;
    private PlayerNetworking _playerNetworking;


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



        _playerNetworkings = FindObjectsOfType<PlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }


        if (SceneManager.GetActiveScene().name == "ArenaRoom")
        {
            ArenaStart.OnTournamentStart.RemoveListener(BettingUIInit);
            ArenaStart.OnTournamentStart.AddListener(BettingUIInit);
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

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            BettingPanelOn();
        }
#endif
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
        for (int i = 0; i < _isBetting.Length; ++i)
        {
            if (_isBetting[i])
            {
                return true;
            }
        }
        return false;
    }


    [PunRPC]
    public void BetChampionAmount(int index, double bettingGold)
    {
        OnBetChampion.Invoke(index, bettingGold);
    }

    private void BetChampion(int index)
    {
        if (MySqlSetting.CheckHaveGold(_playerNetworking.MyNickname) < double.Parse(_betChampionInputField[index].text))
        {
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅액이 부족합니다.";
            _betChampionInputField[index].text = null;

            return;
        }


        MySqlSetting.InsertBetting(_playerNetworking.MyNickname, double.Parse(_betChampionInputField[index].text), index);
        MySqlSetting.UpdateGoldAfterBetting(_playerNetworking.MyNickname, double.Parse(_betChampionInputField[index].text));

        _isBetting[index] = true;

        _betChampionInputField[index].text = null;

        _popUpPanel.SetActive(true);

        _popUpMessage.text = "베팅이 완료되었습니다.";


    }


    [PunRPC]
    public void BetCancelAmount(int index, double cancelGold)
    {
        OnBetCancelChampion.Invoke(index, cancelGold);
    }

    private void BetCancel(int index)
    {
        _isBetting[index] = false;

        _popUpPanel.SetActive(true);

        _popUpMessage.text = "베팅 취소가 완료되었습니다.";
    }

    private void BetChampionOne()
    {
        if (BettingExist() == false)
        {
            photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, 0, double.Parse(_betChampionInputField[0].text));
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
            photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, 1, double.Parse(_betChampionInputField[1].text));
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
            photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, 2, double.Parse(_betChampionInputField[2].text));
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
            photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, 3, double.Parse(_betChampionInputField[3].text));
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
        if (SceneManager.GetActiveScene().name == "ArenaRoom")
        {
            ArenaStart.OnTournamentStart.RemoveListener(BettingUIInit);
        }
    }
}

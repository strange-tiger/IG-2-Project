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

/*
 * 플레이어가 BettingUI와 상호작용하여 투기장 베팅을 할 수 있게 해줌.
 */
public class BettingUI : MonoBehaviourPun
{

    // 베팅과 베팅 취소를 BettingManager에 이벤트로 전달. 베팅한 챔피언의 인덱스와 베팅 금액을 매개변수로 전달.
    public UnityEvent<int, int> OnBetChampion = new UnityEvent<int, int>();
    public UnityEvent<int, int> OnBetCancelChampion = new UnityEvent<int, int>();

    // 베팅 패널 UI.
    [Header("Betting Panel")]
    [SerializeField] private GameObject _bettingPanel;
    [SerializeField] private Button _bettingPanelButton;
    [SerializeField] private Button _bettingPanelOffButton;

    // 베팅이 끝남을 알리는 팝업.
    [Header("Betting End PopUp")]
    [SerializeField] private GameObject _bettingEndPopUpPanel;
    [SerializeField] private Button _bettingPopUpPanelOffButton;

    // 각 챔피언에 인덱스에 해당하는 베팅 버튼.
    [Header("Betting Button")]
    [SerializeField] private Button _betChampionOneButton;
    [SerializeField] private Button _betChampionTwoButton;
    [SerializeField] private Button _betChampionThreeButton;
    [SerializeField] private Button _betChampionFourButton;

    // 각 챔피언에 인덱스에 해당하는 베팅 취소 버튼.
    [Header("Betting Cancel Button")]
    [SerializeField] private Button _betCancelChampionOneButton;
    [SerializeField] private Button _betCancelChampionTwoButton;
    [SerializeField] private Button _betCancelChampionThreeButton;
    [SerializeField] private Button _betCancelChampionFourButton;

    // 베팅, 베팅 취소나 베팅이 불가함을 알리는 팝업.
    [Header("Betting PopUp Panel")]
    [SerializeField] private GameObject _popUpPanel;
    [SerializeField] private TextMeshProUGUI _popUpMessage;
    [SerializeField] private Button _popUpOffButton;

    // 각 챔피언에게 베팅할 금액을 입력 받을 InputField.
    [Header("Betting InputField")]
    [SerializeField] private TMP_InputField[] _betChampionInputField;

    // 각 챔피언의 베팅율을 나타냄.
    [Header("Betting Rate")]
    [SerializeField] public TextMeshProUGUI[] BetRateText;

    // UI와 연결된 베팅 매니저.
    [SerializeField] private BettingManager _bettingManager;

    // 상호작용하는 플레이어의 닉네임을 받아오기 위한 PlayerNetworking.
    private BasicPlayerNetworking[] _playerNetworkings;
    private BasicPlayerNetworking _playerNetworking;

    // 챔피언에게 베팅을 했는지 여부를 판단하기 위한 Boolean배열.
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

        // 베팅이 시작함을 이벤트로 전달 받아 BettingUI를 초기화하여 띄움.
        ArenaStart.OnTournamentStart.RemoveListener(BettingStart);
        ArenaStart.OnTournamentStart.AddListener(BettingStart);

        // 상호 작용하는 플레이어의 Nickname을 받기위해 PlayerNetworking을 찾아옴.
        _playerNetworkings = FindObjectsOfType<BasicPlayerNetworking>();

        foreach (var player in _playerNetworkings)
        {
            if (player.GetComponent<PhotonView>().IsMine)
            {
                _playerNetworking = player;
            }
        }

        // 각 InputField에 가상 넘버패드를 붙여줌.
        foreach (TMP_InputField inputfield in _betChampionInputField)
        {
            inputfield.onSelect.AddListener((string temp) =>
                {
                    KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
                }
            );
        }
    }

    /// <summary>
    /// 베팅 UI와 베팅 매니저를 연결하고, 베팅 UI를 띄울 수 있는 버튼을 활성화.
    /// 베팅율과 베팅 여부를 초기화.
    /// </summary>
    private void BettingUIInit()
    {
        _bettingManager = FindObjectOfType<BettingManager>();
        _bettingPanelButton.gameObject.SetActive(true);

        for (int i = 0; i < BetRateText.Length; ++i)
        {
            BetRateText[i].text = null;
            _isBetting[i] = false;
        }
    }

    private void PopUpPanelOff() => _popUpPanel.SetActive(false);

    /// <summary>
    ///  베팅이 시작하면 BettingUIInit을 호출.
    /// </summary>
    private void BettingStart()
    {
        BettingUIInit();
    }

    /// <summary>
    /// BettingUI를 띄워주고 버튼을 비활성화 시킴.
    /// </summary>
    private void BettingPanelOn()
    {
        _bettingPanelButton.gameObject.SetActive(false);
        _bettingPanel.SetActive(true);
    }

    /// <summary>
    /// BettingUI를 닫고 버튼을 활성화 시킴.
    /// </summary>
    private void BettingPanelOff()
    {
        _bettingPanelButton.gameObject.SetActive(true);
        _bettingPanel.SetActive(false);
    }

    /// <summary>
    /// 플레이어의 베팅이 존재함을 확인함. 베팅이 존재하면 취소하기 전까지 다른 베팅을 할 수 없음.
    /// </summary>
    /// <returns>베팅이 하나라도 존재하면 true, 아니면 false를 반환</returns>
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

    /// <summary>
    /// 각 챔피언의 InputField를 초기화 시킴.
    /// </summary>
    private void InputFieldClear()
    {
        for (int i = 0; i < _betChampionInputField.Length; ++i)
        {
            _betChampionInputField[i].text = null;
        }
    }

    /// <summary>
    /// 베팅한 챔피언의 인덱스와 베팅 금액을 베팅 매니저에 전달함.
    /// </summary>
    /// <param name="index">베팅한 챔피언의 인덱스</param>
    /// <param name="bettingGold">베팅한 금액</param>
    [PunRPC]
    public void BetChampionAmount(int index, int bettingGold)
    {
        OnBetChampion.Invoke(index, bettingGold);
    }

    /// <summary>
    /// 인덱스를 받아 DB에 베팅 정보를 저장.
    /// 소지 금액이 충분한지 확인하고, 알맞는 팝업을 띄움.
    /// </summary>
    /// <param name="index">베팅한 챔피언의 인덱스</param>
    private void BetChampion(int index)
    {
        // 플레이어의 소지 골드를 확인하여, 베팅하려는 금액보다 소지금액이 적으면
        if (MySqlSetting.CheckHaveGold(_playerNetworking.MyNickname) < int.Parse(_betChampionInputField[index].text))
        {
            // 베팅 금액이 부족함을 알리는 팝업을 띄우고
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅액이 부족합니다.";
            
            // InputField를 초기화.
            InputFieldClear();

            // 반환.
            return;
        }

        // 베팅 금액이 충분하다면 BettingDB에 베팅 정보를 추가하고, 플레이어의 골드 소지량도 업데이트함.
        MySqlSetting.InsertBetting(_playerNetworking.MyNickname, int.Parse(_betChampionInputField[index].text), index);
        MySqlSetting.UpdateGoldAfterBetting(_playerNetworking.MyNickname, int.Parse(_betChampionInputField[index].text));

        // 베팅 여부를 true로 바꿔줌.
        _isBetting[index] = true;

        // MasterClient의 베팅 매니저에만 이벤트를 전달.
        photonView.RPC("BetChampionAmount", RpcTarget.MasterClient, index, int.Parse(_betChampionInputField[index].text));

        // InputField 초기화.
        InputFieldClear();

        // 베팅이 완료되었음을 알리는 팝업 출력.
        _popUpPanel.SetActive(true);

        _popUpMessage.text = "베팅이 완료되었습니다.";
    }

    /// <summary>
    /// 1번 챔피언에 베팅.
    /// 베팅이 존재하면 베팅을 취소해야 베팅이 가능함을 알리는 팝업을 출력하고, 존재하지 않으면 BetChampion을 호출.
    /// </summary>
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

    /// <summary>
    /// 베팅이 취소되었음을 베팅 매니저에 전달하는 이벤트를 호출.
    /// </summary>
    /// <param name="index"> 베팅을 취소할 챔피언의 인덱스</param>
    /// <param name="cancelGold"> 취소할 베팅의 베팅 금액</param>
    [PunRPC]
    public void BetCancelAmount(int index, int cancelGold)
    {
        OnBetCancelChampion.Invoke(index, cancelGold);
    }

    /// <summary>
    /// 챔피언의 인덱스를 받아 베팅 여부를 false로 만들고, 베팅이 취소되었음을 알리는 팝업 출력.
    /// </summary>
    /// <param name="index"></param>
    private void BetCancel(int index)
    {
        // 받은 챔피언의 베팅 여부를 false로 만들어줌.
        _isBetting[index] = false;

        // InputField를 초기화.
        InputFieldClear();

        // 취소 완료를 알리는 팝업 출력.
        _popUpPanel.SetActive(true);

        _popUpMessage.text = "베팅 취소가 완료되었습니다.";
    }

    /// <summary>
    /// 1번 챔피언의 베팅을 취소함.
    /// </summary>
    private void BetCancelChampionOne()
    {
        // 플레이어가 1번 챔피언에 베팅 했었어야만 취소가 가능.
        if (_isBetting[0])
        {
            // MasterClient의 베팅 매니저에게 베팅한 챔피언의 인덱스와 BettingDB에 저장되어 있던 금액을 이벤트로 전달.
            photonView.RPC("BetCancelAmount", RpcTarget.MasterClient, 0, MySqlSetting.CancelBetting(_playerNetworking.MyNickname));
            BetCancel(0);
        }
        else
        {
            // 아니라면 베팅 내역이 없음을 알리는 팝업출력.
            _popUpPanel.SetActive(true);
            _popUpMessage.text = "베팅 내역이 없습니다.";
        }
    }
    private void BetCancelChampionTwo()
    {
        if (_isBetting[1])
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
        if (_isBetting[2])
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
        if (_isBetting[3])
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

    /// <summary>
    /// 경기가 시작하면 베팅이 끝남을 알리고 베팅 패널을 비활성화 시킴.
    /// </summary>
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

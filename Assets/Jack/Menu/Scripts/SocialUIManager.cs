using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

public class SocialUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _socialUI;
    [SerializeField] private TextMeshProUGUI _targetUserNicknameText;

    [Header("Add Friend")]
    [SerializeField] private Button _addFriendButton;
    [SerializeField] private string _requestFriendConfirmMessage;
    [SerializeField] private string _cancelRequestConfirmMessage;
    [SerializeField] private string _cancelFriendConfirmMessage;

    [Header("Check Request")]
    [SerializeField] private string _checkRequestMessage;
    [SerializeField] private string _acceptRequestMessage;
    [SerializeField] private string _denyRequestMessage;

    [Header("Block User")]
    [SerializeField] private Button _blockFriendButton;
    [SerializeField] private string _blockConfirmMessage;
    [SerializeField] private string _unblockConfirmMessage;
    private TextMeshProUGUI _blockFriendText;
    [SerializeField] private GameObject _blockedConfirmedPanel;

    [Header("Extra")]
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;
    [SerializeField] private CheckPanelManager _checkPanelManager;

    private string _myNickname;
    private string _targetUserNickname;

    private void Awake()
    {
        _myNickname = TempAccountDB.Nickname;
        _blockFriendText = _blockFriendButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // 1. 둘 사이의 관계를 확인함

        // 1-1. 관계가 없는 경우(반환값이 -1)
        OnClickRequestFriendButton();
        OnClickBlockUserButton();

        // 1-2. 친구인 경우(반환값이 0)
        OnClickCancelFriendButton();
        OnClickBlockUserButton();

        // ....그외
        // 2. 둘 사이에서 나의 위치(A인지 B인지 알아냄)를 확인하여
        // 위치에 따라 확인할 비트 자리수를 기준을 설정, 해당 결과로 다음을 판별

        // 2-1. 내가 블록한 상황
        ;
        OnClickUnblockUserButton();

        // 2-2. 내가 친구 요청을 한 상황
        OnClickCancelRequestButton();
        OnClickBlockUserButton();

        // 2-3. 상태가 나에게 친구 요청을 한 상황
        OnClickCheckFriendRequestButton();
        OnClickBlockUserButton();
    }

    /// <summary>
    /// 친구 추가 패널을 보여줌
    /// </summary>
    /// <param name="targetUserName"> 타겟 유저 이름 </param>
    public void ShowFriendPanel(string targetUserName)
    {
        _targetUserNickname = targetUserName;
        _targetUserNicknameText.text = _targetUserNickname;
        _socialUI.SetActive(true);
    }

    private void AddListenerToButton(Button button, UnityEngine.Events.UnityAction newListener, bool isNeedButtonInitialize)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(newListener);
        button.onClick.AddListener(() => { InitializeButtons(); });
    }

    // 친구 추가 요청
    private void OnClickRequestFriendButton()
    {
        // 친구 추가 요청을 DB에 올림
        _confirmPanelManager.ShowConfirmPanel(_requestFriendConfirmMessage);
    }

    // 친구 추가 요청 취소
    private void OnClickCancelRequestButton()
    {
        // 친구 취소 요청을 DB에 올림
        _confirmPanelManager.ShowConfirmPanel(_cancelRequestConfirmMessage);
    }

    // 친구 삭제
    private void OnClickCancelFriendButton()
    {
        // 친구 삭제 요청을 DB에 올림
        _confirmPanelManager.ShowConfirmPanel(_cancelFriendConfirmMessage);
    }

    // 상대의 친구 요청 판단
    private void OnClickCheckFriendRequestButton()
    {
        _checkPanelManager.ShowCheckPanel(_targetUserNickname + _checkRequestMessage,
            () =>
            {
                // 친구 추가 요청을 DB에 올림
                _confirmPanelManager.ShowConfirmPanel(_acceptRequestMessage);
                InitializeButtons();
            },
            () =>
            {
                // 친구 삭제 요청을 DB에 올림
                _confirmPanelManager.ShowConfirmPanel(_denyRequestMessage);
                InitializeButtons();
            }
            );
    }
    
    // 상대 차단
    private void OnClickBlockUserButton()
    {

    }

    // 상대 차단 해제
    private void OnClickUnblockUserButton()
    {

    }
    /*
    switch (MySqlSetting.CheckSocialStatus(_myNickname, _targetUserNickname))
    {
        // 1. 친구 요청 상태일 경우
        case ESocialStatus.Request:
            {
                // 친구 요청 다시 못함
                _addFriendButton.interactable = false;

                // 블록 기능은 가능
                _blockFriendButton.onClick.AddListener(() =>
               {
                   MySqlSetting.UpdateSocialStatus(_myNickname, _targetUserNickname, ESocialStatus.Block);
                   _blockedConfirmedPanel.SetActive(true);
                   _socialUI.SetActive(false);
               });
            }
            break;

        // 2. 블록 상태일 경우
        case ESocialStatus.Block:
            {
                _addFriendButton.interactable = false;

                // 블록 버튼은 블록 해제 버튼으로 변함
                _blockFriendText.text = _blockDissableInfoString;
                _blockFriendButton.onClick.AddListener(() =>
               {
                   // 블록 해제(RelationshipDB에서 해당 관계 삭제) 기능 추가 필요
                   Debug.Log("블록 해제함");
                   _blockFriendText.text = _blockInfoString;

                   // 버튼 초기화
                   InitializeButtons();
               });
            }
            break;

        // 3. 그냥 상태일 경우
        default:
            {
                _addFriendButton.interactable = true;

                _blockFriendButton.interactable = true;
            }
            break;
    }
    */
}

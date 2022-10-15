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
    private bool _isTargetUserNicknameSet;

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
        // 0. 사전 조건 판별: 대상 유저의 Nickname을 받음
        Debug.Assert(_isTargetUserNicknameSet, "상대 닉네임 없음");

        // 1. 둘 사이의 관계를 확인함
        bool isLeft;
        int relationship = MySqlSetting.CheckRelationship(_myNickname, _targetUserNickname, out isLeft);

        // 1-1. 관계가 없는 경우(반환값이 -1)
        if(relationship == -1)
        {
            // 친구 요청
            _addFriendButton.interactable = true;
            AddListenerToButton(_addFriendButton, OnClickRequestFriendButton, true);

            // 블록
            _blockFriendButton.interactable = true;
            AddListenerToButton(_blockFriendButton, OnClickBlockUserButton, true);
        }

        // 1-2. 친구인 경우(반환값이 0)
        else if(relationship == 0)
        {
            // 친구 취소
            _addFriendButton.interactable = true;
            AddListenerToButton(_addFriendButton, OnClickCancelFriendButton, true);

            // 블록
            _blockFriendButton.interactable = true;
            AddListenerToButton(_blockFriendButton, OnClickBlockUserButton, true);
        }

        // ....그외
        else
        {
            // 2. 둘 사이에서 나의 위치(A인지 B인지 알아냄)를 확인하여
            // 위치에 따라 확인할 비트 자리수를 기준을 설정, 해당 결과로 다음을 판별
            int parseBinary = 0b_0011;

            if (isLeft)
            {
                relationship = relationship >> 2;
            }
            int state = relationship & parseBinary;

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
    }

    /// <summary>
    /// 친구 추가 패널을 보여줌
    /// </summary>
    /// <param name="targetUserName"> 타겟 유저 이름 </param>
    public void ShowFriendPanel(string targetUserName)
    {
        _targetUserNickname = targetUserName;
        _targetUserNicknameText.text = _targetUserNickname;
        _isTargetUserNicknameSet = true;
        _socialUI.SetActive(true);
    }

    /// <summary>
    /// 버튼에 리스너를 부착함
    /// </summary>
    /// <param name="button">부착 대상 버튼</param>
    /// <param name="newListener">부착할 리스너</param>
    /// <param name="isNeedExitPanelListener">클릭 후 버튼 초기화가 필요한지 여부</param>
    private void AddListenerToButton(Button button, UnityEngine.Events.UnityAction newListener, bool isNeedExitPanelListener)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(newListener);
        if(isNeedExitPanelListener)
        {
            button.onClick.AddListener(() => { _socialUI.SetActive(false); });
        }
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
        // 상대 차단
        _confirmPanelManager.ShowConfirmPanel(_blockConfirmMessage);
    }

    // 상대 차단 해제
    private void OnClickUnblockUserButton()
    {
        // 차단 해제
        _confirmPanelManager.ShowConfirmPanel(_unblockConfirmMessage);
    }
}

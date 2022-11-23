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
    [SerializeField] private Button _friendButton;
    private TextMeshProUGUI _friendButtonText;
    
    [SerializeField] private string _requestFriendText;
    [SerializeField] private string _requestFriendConfirmMessage;

    [SerializeField] private string _cancelRequestText;
    [SerializeField] private string _cancelRequestConfirmMessage;
    
    [SerializeField] private string _cancelFriendText;
    [SerializeField] private string _cancelFriendConfirmMessage;

    [Header("Check Request")]
    [SerializeField] private string _checkRequestText;
    [SerializeField] private string _checkRequestMessage;

    [SerializeField] private string _acceptRequestMessage;
    [SerializeField] private string _denyRequestMessage;

    [Header("Block User")]
    [SerializeField] private Button _blockButton;
    private TextMeshProUGUI _blockButtonText;
    
    [SerializeField] private string _blockFriendText;
    [SerializeField] private string _blockConfirmMessage;

    [SerializeField] private string _unblockText;
    [SerializeField] private string _unblockConfirmMessage;

    [Header("Extra")]
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;
    [SerializeField] private CheckPanelManager _checkPanelManager;

    private string _myNickname;

    private UserInteraction _targetUser;
    private string _targetUserNickname;
    private bool _isTargetUserNicknameSet;

    private void Awake()
    {
        _myNickname = TempAccountDB.Nickname;
        _blockButtonText = _blockButton.GetComponentInChildren<TextMeshProUGUI>();
        _friendButtonText = _friendButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        // 0. 사전 조건 판별: 대상 유저의 Nickname을 받음
        //Debug.Assert(_isTargetUserNicknameSet, "상대 닉네임 없음");
        if(!_isTargetUserNicknameSet)
        {
            return;
        }

        // 1. 둘 사이의 관계를 확인함
        bool isLeft;
        int relationship = MySqlSetting.CheckRelationship(_myNickname, _targetUserNickname, out isLeft);

        // 1-1. 관계가 없는 경우(반환값이 -1)
        if(relationship == -1)
        {
            // 친구 요청
            SetButton(_friendButton, true, _friendButtonText, _requestFriendText);
            AddListenerToButton(_friendButton, OnClickRequestFriendButton, true);

            // 차단
            SetButton(_blockButton, true, _blockButtonText, _blockFriendText);
            AddListenerToButton(_blockButton, OnClickBlockUserButton, true);
        }

        // 1-2. 친구인 경우(반환값이 0)
        else if(relationship == 0)
        {
            // 친구 취소
            SetButton(_friendButton, true, _friendButtonText, _cancelFriendText);
            AddListenerToButton(_friendButton, OnClickCancelFriendButton, true);

            // 차단
            SetButton(_blockButton, true, _blockButtonText, _blockFriendText);
            AddListenerToButton(_blockButton, OnClickBlockUserButton, true);
        }

        // ....그외
        else
        {
            // 2. 둘 사이에서 나의 위치(A인지 B인지 알아냄)를 확인하여
            // 위치에 따라 확인할 비트 자리수를 기준을 설정, 해당 결과로 다음을 판별
            bool hasBlock; // 내가 블록했는지
            bool hasRequest; // 내가 요청했는지
            bool isFriendRequested; // 내가 요청받았는지
            if(isLeft)
            {
                hasBlock = (relationship & MySqlSetting._BLOCK_LEFT_BIT) == MySqlSetting._BLOCK_LEFT_BIT;
                hasRequest = (relationship & MySqlSetting._REQUEST_LEFT_BIT) != 0;
                isFriendRequested = (relationship & MySqlSetting._REQUEST_RIGHT_BIT) != 0;
            }
            else
            {
                hasBlock = (relationship & MySqlSetting._BLOCK_RIGHT_BIT) == MySqlSetting._BLOCK_RIGHT_BIT;
                hasRequest = (relationship & MySqlSetting._REQUEST_RIGHT_BIT) != 0;
                isFriendRequested = (relationship & MySqlSetting._REQUEST_LEFT_BIT) != 0;
            }

            // 2-1. 내가 블록한 상황
            if(hasBlock)
            {
                // 친구 추가 버튼이 interactable false(기능 없어도 됨)
                SetButton(_friendButton, false, _friendButtonText, _requestFriendText);

                // 차단 취소
                SetButton(_blockButton, true, _blockButtonText, _unblockText);
                AddListenerToButton(_blockButton, OnClickUnblockUserButton, true);
            }

            // 2-2. 내가 친구 요청을 한 상황
            else if(hasRequest)
            {
                // 친구 추가 취소
                SetButton(_friendButton, true, _friendButtonText, _cancelRequestText);
                AddListenerToButton(_friendButton, OnClickCancelRequestButton, true);

                // 차단
                SetButton(_blockButton, true, _blockButtonText, _blockFriendText);
                AddListenerToButton(_blockButton, OnClickBlockUserButton, true);
            }

            // 2-3. 상태가 나에게 친구 요청을 한 상황
            else if(isFriendRequested)
            {
                // 요청 판단
                SetButton(_friendButton, true, _friendButtonText, _checkRequestText);
                AddListenerToButton(_friendButton, OnClickCheckFriendRequestButton, true);

                // 차단
                SetButton(_blockButton, true, _blockButtonText, _blockFriendText);
                AddListenerToButton(_blockButton, OnClickBlockUserButton, true);
            }
        }
    }

    /// <summary>
    /// 친구 추가 패널을 보여줌
    /// </summary>
    /// <param name="targetUser"> 타겟 유저 이름 </param>
    public void ShowFriendPanel(UserInteraction targetUser)
    {
        _targetUser = targetUser;
        _targetUserNickname = targetUser.Nickname;
        _targetUserNicknameText.text = _targetUserNickname;
        _isTargetUserNicknameSet = true;
        InitializeButtons();
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
        if(isNeedExitPanelListener)
        {
            button.onClick.AddListener(() => { _socialUI.SetActive(false); });
        }
        button.onClick.AddListener(newListener);
    }

    /// <summary>
    /// 버튼 세팅
    /// </summary>
    /// <param name="button">버튼 본체</param>
    /// <param name="interactable">버튼 interactable 설정</param>
    /// <param name="buttonText">버튼의 TextMeshPro</param>
    /// <param name="buttonInfoText">버튼의 안내 메시지</param>
    private void SetButton(Button button, bool interactable, TextMeshProUGUI buttonText, string buttonInfoText)
    {
        button.interactable = interactable;
        buttonText.text = buttonInfoText;
    }

    // 친구 추가 요청
    private void OnClickRequestFriendButton()
    {
        // 친구 추가 요청을 DB에 올림
        MySqlSetting.UpdateRelationshipToRequest(_myNickname, _targetUserNickname);
        _targetUser.photonView.RPC("SendRequest", Photon.Pun.RpcTarget.All, _myNickname);

        // 확인 메시지 출력
        _confirmPanelManager.ShowConfirmPanel(_requestFriendConfirmMessage);
    }

    // 친구 추가 요청 취소
    private void OnClickCancelRequestButton()
    {
        // 친구 요청 취소를 DB에 올림
        MySqlSetting.UpdateRelationshipToUnrequest(_myNickname, _targetUserNickname);

        // 확인 메시지 출력
        _confirmPanelManager.ShowConfirmPanel(_cancelRequestConfirmMessage);
    }

    // 친구 삭제
    private void OnClickCancelFriendButton()
    {
        // 친구 삭제 요청을 DB에 올림
        MySqlSetting.UpdateRelationshipToUnFriend(_myNickname, _targetUserNickname);

        // 확인 메시지 출력
        _confirmPanelManager.ShowConfirmPanel(_cancelFriendConfirmMessage);
    }

    // 상대의 친구 요청 판단
    private void OnClickCheckFriendRequestButton()
    {
        // 판단 페널 출력
        _checkPanelManager.ShowCheckPanel(_targetUserNickname + _checkRequestMessage,
            () =>
            {
                // 친구 추가을 DB에 올림
                MySqlSetting.UpdateRelationshipToFriend(_myNickname, _targetUserNickname);

                // 확인 메시지 출력
                _confirmPanelManager.ShowConfirmPanel(_acceptRequestMessage);
                InitializeButtons();
            },
            () =>
            {
                // 친구 요청 삭제을 DB에 올림
                MySqlSetting.UpdateRelationshipToUnrequest(_targetUserNickname, _myNickname);

                // 확인 메시지 출력
                _confirmPanelManager.ShowConfirmPanel(_denyRequestMessage);

                InitializeButtons();
            }
            );
    }
    
    // 상대 차단
    private void OnClickBlockUserButton()
    {
        // 상대 차단
        MySqlSetting.UpdateRelationshipToBlock(_myNickname, _targetUserNickname);

        // 확인 메시지 출력
        _confirmPanelManager.ShowConfirmPanel(_blockConfirmMessage);
    }

    // 상대 차단 해제
    private void OnClickUnblockUserButton()
    {
        // 차단 해제
        MySqlSetting.UpdateRelationshipToUnblock(_myNickname, _targetUserNickname);

        // 확인 메시지 출력
        _confirmPanelManager.ShowConfirmPanel(_unblockConfirmMessage);
    }
}

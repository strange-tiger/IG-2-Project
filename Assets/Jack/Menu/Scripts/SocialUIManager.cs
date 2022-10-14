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

    [Header("Block User")]
    [SerializeField] private Button _blockFriendButton;
    [SerializeField] private string _blockInfoString;
    [SerializeField] private string _blockDissableInfoString;
    private TextMeshProUGUI _blockFriendText;
    [SerializeField] private GameObject _blockedConfirmedPanel;

    [Header("Extra")]
    [SerializeField] private ConfirmPanelManager _confirmPanelManager;

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
        RequestFriendButton();
        BlockUserButton();

        // 1-2. 친구인 경우(반환값이 0)
        CancelFriendButton();
        BlockUserButton();

        // ....그외
        // 2. 둘 사이에서 나의 위치(A인지 B인지 알아냄)를 확인하여
        // 위치에 따라 확인할 비트 자리수를 기준을 설정, 해당 결과로 다음을 판별
        
        // 2-1. 내가 블록한 상황
        ;
        UnblockUserButton();

        // 2-2. 내가 친구 요청을 한 상황
        CancelRequestButton();
        BlockUserButton();

        // 2-3. 상태가 나에게 친구 요청을 한 상황
        CheckFriendRequestButton();
        BlockUserButton();
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

    private void RequestFriendButton()
    {

    }

    private void CancelRequestButton()
    {

    }

    private void CancelFriendButton()
    {

    }

    private void CheckFriendRequestButton()
    {

    }
    
    private void BlockUserButton()
    {

    }

    private void UnblockUserButton()
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

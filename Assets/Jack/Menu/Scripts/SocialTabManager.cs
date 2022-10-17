using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

public class SocialTabManager : MonoBehaviour
{
    [Header("List View")]
    [SerializeField] private GameObject _friendListItem;
    [SerializeField] private GameObject _requestListItem;
    [SerializeField] private GameObject _listContent;

    [SerializeField] private float _listUpdateOffsetTime;
    private WaitForSeconds _listUpdateWaitForSeconds;

    [Header("Buttons")]
    [SerializeField] private Button _friendListButton;
    [SerializeField] private Button _blockListButton;
    [SerializeField] private Button _requestListButton;

    [SerializeField] private Color _onLineTextColor;
    [SerializeField] private Color _offLineTextColor;

    private SocialUIManager _socialUIManager;
    private string _myNickname;

    private List<TextMeshProUGUI> _nicknameTextList = new List<TextMeshProUGUI>();

    private delegate void AddListItem(string);

    private void Awake()
    {
        _myNickname = TempAccountDB.Nickname;
        _listUpdateWaitForSeconds = new WaitForSeconds(_listUpdateOffsetTime);
    }

    private void OnEnable()
    {
        ShowFriendList();
    }

    private void setButtons()
    {
        _friendListButton.onClick.AddListener(ShowFriendList);
        _blockListButton.onClick.AddListener(ShowBlockList);
        _requestListButton.onClick.AddListener(ShowRequestList);
    }

    private void ShowFriendList()
    {
        // 버튼 활성화 처리
        _friendListButton.interactable = false;
        _blockListButton.interactable = true;
        _requestListButton.interactable = true;

        // 리스트 세팅
        SetList(MySqlSetting._FRIEND_BIT, MySqlSetting._FRIEND_BIT, AddFriendListItem);
    }

    private void ShowBlockList()
    {
        // 버튼 활성화 처리
        _friendListButton.interactable = true;
        _blockListButton.interactable = false;
        _requestListButton.interactable = true;

        // 리스트 세팅
        SetList(MySqlSetting._BLOCK_LEFT_BIT, MySqlSetting._BLOCK_RIGHT_BIT, AddBlockListItem);
    }
    
    private void ShowRequestList()
    {
        // 버튼 활성화 처리
        _friendListButton.interactable = true;
        _blockListButton.interactable = true;
        _requestListButton.interactable = false;

        // 리스트 세팅
        SetList(MySqlSetting._REQUEST_LEFT_BIT, MySqlSetting._REQUEST_RIGHT_BIT, AddRequestListItem);
    }

    private void SetList(byte leftListOptionByte, byte rightListOptionByte, AddListItem addListItem)
    {
        ResetList();
        List<Dictionary<string, string>> relationShipList = MySqlSetting.GetRelationList(_myNickname);

        foreach(Dictionary<string, string> relationship in relationShipList)
        {
            byte optionByte;
            if(bool.Parse(relationship["IsLeft"]))
            {
                optionByte = leftListOptionByte;
            }
            else
            {
                optionByte = rightListOptionByte;
            }

            if((byte.Parse(relationship["State"]) & optionByte) == optionByte)
            {
                addListItem.Invoke(relationship["Nickname"]);
            }
        }
    }

    private void ResetList()
    {
        _nicknameTextList.Clear();
        foreach(Transform child in _listContent.GetComponentsInChildren<Transform>())
        {
            if(child.gameObject == _listContent)
            {
                continue;
            }

            Destroy(child);
        }
    }

    private void AddFriendListItem(string targetNickname)
    {
        GameObject newFriendListItem = Instantiate(_friendListItem, _listContent.transform);

        _nicknameTextList.Add(newFriendListItem.GetComponentInChildren<TextMeshProUGUI>());
        _nicknameTextList[_nicknameTextList.Count - 1].text = targetNickname;
        
        // 친구 삭제
        Button deleteButton = newFriendListItem.GetComponentInChildren<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            // 친구 삭제 기능 연결
            MySqlSetting.UpdateRelationshipToUnFriend(_myNickname, targetNickname);
        });
    }
    private void AddBlockListItem(string targetNickname)
    {
        GameObject newFriendListItem = Instantiate(_friendListItem, _listContent.transform);

        _nicknameTextList.Add(newFriendListItem.GetComponentInChildren<TextMeshProUGUI>());
        _nicknameTextList[_nicknameTextList.Count - 1].text = targetNickname;

        // 차단 취소
        Button deleteButton = newFriendListItem.GetComponentInChildren<Button>();
        deleteButton.onClick.AddListener(() =>
        {
            // 차단 취소 기능 연결
            MySqlSetting.UpdateRelationshipToUnblock(_myNickname, targetNickname);
        });
    }
    private void AddRequestListItem(string targetNickname)
    {
        GameObject newFriendListItem = Instantiate(_requestListItem, _listContent.transform);

        _nicknameTextList.Add(newFriendListItem.GetComponentInChildren<TextMeshProUGUI>());
        _nicknameTextList[_nicknameTextList.Count - 1].text = targetNickname;

        // 친구 수락
        Button acceptButton = newFriendListItem.GetComponentsInChildren<Button>()[0];
        acceptButton.onClick.AddListener(() =>
        {
            // 친구 추가 기능 연결
            MySqlSetting.UpdateRelationshipToFriend(targetNickname, _myNickname);
        });

        // 친구 거절
        Button denyButton = newFriendListItem.GetComponentsInChildren<Button>()[1];
        acceptButton.onClick.AddListener(() =>
        {
            // 친구 거절 기능 연결
            MySqlSetting.UpdateRelationshipToUnrequest(targetNickname, _myNickname);
        });
    }
}

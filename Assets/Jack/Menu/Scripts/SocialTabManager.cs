using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;

public class SocialTabManager : MonoBehaviour
{
    [Header("List View")]
    [SerializeField] private GameObject _friendPanel;
    [SerializeField] private GameObject _requestPanel;
    [SerializeField] private GameObject _listContent;

    [SerializeField] private float _listUpdateOffsetTime;
    private WaitForSeconds _listUpdateWaitForSeconds;

    [Header("Buttons")]
    [SerializeField] private Button _friendListButton;
    [SerializeField] private Button _blockListButton;
    [SerializeField] private Button _requestListButton;

    private SocialUIManager _socialUIManager;
    private byte _currentListState;


    private void Awake()
    {
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
        //_currentListState = ESocialStatus.Friend;
    }

    private void ShowBlockList()
    {

    }
    
    private void ShowRequestList()
    {

    }

    private IEnumerator ShowList()
    {
        while(gameObject.activeSelf)                                                                                                                                                              
        {
            List<Dictionary<string, string>> list;

            yield return _listUpdateWaitForSeconds;
        }
    }


}

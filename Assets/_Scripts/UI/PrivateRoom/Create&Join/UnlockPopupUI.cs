using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

using _PH = ExitGames.Client.Photon;
using _DB = Asset.MySql.MySqlSetting;

public class UnlockPopupUI : PopupUI
{
    [SerializeField] Button _joinButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _passwordInput;

    [Header("Popup")]
    [SerializeField] GameObject _errorPopup;

    private string _currentRoomName = string.Empty;
    private string _currentRoomInfo = string.Empty;

    protected override void OnEnable()
    {
        base.OnEnable();
        _joinButton.onClick.RemoveListener(JoinLockedRoom);
        _joinButton.onClick.AddListener(JoinLockedRoom);

        _errorPopup.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _joinButton.onClick.RemoveListener(JoinLockedRoom);

        _passwordInput.text = string.Empty;
    }

    public void PopupUnlock(string room, string info)
    {
        gameObject.SetActive(true);
        SetRoom(room, info);
    }

    private void SetRoom(string room, string info)
    {
        _currentRoomName = room;
        _currentRoomInfo = info;
    }

    private void JoinLockedRoom()
    {
        _PH.Hashtable expectedCustomRoomProperties = new _PH.Hashtable() 
        { 
            { "roomname", _currentRoomName }, 
            { "password", _passwordInput.text },
            { "displayname", _currentRoomInfo }
        };

        try
        {
            JoinRoom.JoinInRoom(expectedCustomRoomProperties);

            _passwordInput.text = string.Empty;
            gameObject.SetActive(false);
        }
        catch
        {
            _errorPopup.SetActive(true);
            Debug.LogError("암호 있는 방 입장 실패");
        }
    }
}
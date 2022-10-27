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

    private string _currentRoomName = "";

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

        _passwordInput.text = "";
    }

    public void PopupUnlock(string room)
    {
        gameObject.SetActive(true);
        SetRoom(room);
    }

    private void SetRoom(string room)
    {
        _currentRoomName = room;
    }

    private void JoinLockedRoom()
    {
        _PH.Hashtable expectedCustomRoomProperties = new _PH.Hashtable() { { "roomname", _currentRoomName + "_" + _passwordInput.text }, { "password", _passwordInput.text } };

        try
        {
            JoinRoom.JoinInRoom(expectedCustomRoomProperties);

            _passwordInput.text = "";
            gameObject.SetActive(false);
        }
        catch
        {
            _errorPopup.SetActive(true);
            Debug.LogError("암호 있는 방 입장 실패");
        }
    }
}
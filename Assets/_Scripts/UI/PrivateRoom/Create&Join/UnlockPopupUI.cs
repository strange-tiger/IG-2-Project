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
    private string _currentRoomPassword = string.Empty;

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

    /// <summary>
    /// 오브젝트를 활성화한다.
    /// SetRoom을 호출해 전달받은 방 정보를 할당한다.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="password"></param>
    public void PopupUnlock(string room, string password)
    {
        gameObject.SetActive(true);
        SetRoom(room, password);
    }

    /// <summary>
    /// 방 이름과 비밀번호를 할당한다.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="password"></param>
    private void SetRoom(string room, string password)
    {
        _currentRoomName = room;
        _currentRoomPassword = password;
    }

    /// <summary>
    /// 저장된 비밀번호와 _passwordInput에 입력받은 비밀번호가 일치하지 않으면 _errorPopup을 활성화하고 return한다.
    /// 일치하면 JoinRoom.JoinInRoom를 호출하고 방 정보를 전달한다.
    /// </summary>
    private void JoinLockedRoom()
    {
        if (!_currentRoomPassword.Equals(_passwordInput.text))
        {
            _errorPopup.SetActive(true);
            return;
        }

        _PH.Hashtable expectedCustomRoomProperties = new _PH.Hashtable() 
        { 
            { "roomname", _currentRoomName }, 
            { "password", _currentRoomPassword }
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
        }
    }
}
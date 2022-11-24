using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using _PH = ExitGames.Client.Photon;

public class RoomInfoTextUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _text;

    [Header("Lock")]
    [SerializeField] GameObject _lockImage;

    [Header("Button")]
    [SerializeField] UnityEngine.UI.Button _button;

    [Header("Popup")]
    [SerializeField] UnlockPopupUI _popup;
    [SerializeField] GameObject _errorPopup;

    private string _roomName = string.Empty;
    private string _roomInfo = string.Empty;
    private string _roomPassword = string.Empty;
    private bool _isLocked = false;

    private void Awake()
    {
        _lockImage.SetActive(false);
    }

    private void OnEnable()
    {
        _button.onClick.RemoveListener(JoinInRoom);
        _button.onClick.AddListener(JoinInRoom);

        _popup.gameObject.SetActive(false);
        _errorPopup.SetActive(false);

        UpdateRoomInfo();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(JoinInRoom);
    }

    public void UpdateRoomInfo()
    {
        _text.gameObject.SetActive(true);
        _text.text = _roomInfo;
        _lockImage.SetActive(_isLocked);
    }

    public void SetRoom(string room)
    {
        _roomName = room;
    }

    public void SetInfo(string info)
    {
        _roomInfo = info;
    }

    public void SetLock(bool isLocked, string password)
    {
        _isLocked = isLocked;
        _roomPassword = password;
    }

    private void JoinInRoom()
    {
        if (_isLocked)
        {
            _popup.PopupUnlock(_roomName, _roomPassword);
            return;
        }

        _PH.Hashtable expectedCustomRoomProperties = new _PH.Hashtable() 
        { 
            { "roomname", _roomName }, 
            { "password", string.Empty }
        };

        try
        {
            JoinRoom.JoinInRoom(expectedCustomRoomProperties);
        }
        catch
        {
            _errorPopup.SetActive(true);
            Debug.LogError("암호 없는 방 입장 실패");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RoomInfoTextUI : MonoBehaviourPunCallbacks
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

    private string _roomName = "";
    private string _roomInfo = "";
    private bool _isLocked = false;

    private void Awake()
    {
        _lockImage.SetActive(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        _button.onClick.RemoveListener(JoinRoom);
        _button.onClick.AddListener(JoinRoom);

        _popup.gameObject.SetActive(false);
        _errorPopup.SetActive(false);

        UpdateRoomInfo();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _button.onClick.RemoveListener(JoinRoom);
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

    public void SetLock(bool isLocked)
    {
        _isLocked = isLocked;
    }

    public void JoinRoom()
    {
        if (_isLocked)
        {
            _popup.PopupUnlock(_roomName);
            return;
        }

        try
        {
            PhotonNetwork.JoinRoom(_roomName);
        }
        catch
        {
            _errorPopup.SetActive(true);
            Debug.LogError("암호 없는 방 입장 실패");
        }
    }
}

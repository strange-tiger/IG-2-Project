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
    private string _roomDisplay = string.Empty;
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

    /// <summary>
    /// 버튼 상호작용을 비활성화한다.
    /// </summary>
    public void DeactivateButton()
    {
        _button.interactable = false;
    }

    /// <summary>
    /// 버튼 상호작용을 활성화한다.
    /// </summary>
    public void ActivateButton()
    {
        _button.interactable = true;
    }

    /// <summary>
    /// 오브젝트가 보이는 정보를 업데이트한다.
    /// </summary>
    public void UpdateRoomInfo()
    {
        _text.text = _roomDisplay;
        _lockImage.SetActive(_isLocked);
    }

    /// <summary>
    /// 이 객체가 연결할 방 이름을 할당한다.
    /// 룸 이동에 사용하는 정보이다.
    /// </summary>
    /// <param name="room"></param>
    public void SetRoom(string room)
    {
        _roomName = room;
    }

    /// <summary>
    /// 이 객체가 보일 방 이름을 할당한다.
    /// </summary>
    /// <param name="display"></param>
    public void SetDisplay(string display)
    {
        _roomDisplay = display;
    }

    /// <summary>
    /// 이 객체가 연결할 방이 비밀번호가 있는지, 비밀번호는 무엇인지 할당한다.
    /// 비밀번호는 룸 이동에 사용하는 정보이다.
    /// </summary>
    /// <param name="isLocked"></param>
    /// <param name="password"></param>
    public void SetLock(bool isLocked, string password)
    {
        _isLocked = isLocked;
        _roomPassword = password;
    }

    /// <summary>
    /// 방에 참가한다.
    /// 룸 이동에 사용할 정보를 전달한다.
    /// 비밀번호가 있다면 _popup.PopupUnlock을 호출한다.
    /// 없다면 JoinRoom.JoinInRoom을 호출한다.
    /// </summary>
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
        }
    }
}

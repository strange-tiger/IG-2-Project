using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomInfoTextUI : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TextMeshProUGUI _text;

    [Header("Lock")]
    [SerializeField] GameObject _lockImage;

    [Header("Button")]
    [SerializeField] UnityEngine.UI.Button _button;

    private string _roomName = "";
    private string _roomInfo = "";
    private bool _isLocked = false;

    private void Awake()
    {
        _lockImage.SetActive(false);
    }

    private void OnEnable()
    {
        _button.onClick.RemoveListener(JoinRoom);
        _button.onClick.AddListener(JoinRoom);

        UpdateRoomInfo();
    }

    private void OnDisable()
    {
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
        PhotonNetwork.JoinRoom(_roomName);
    }
}

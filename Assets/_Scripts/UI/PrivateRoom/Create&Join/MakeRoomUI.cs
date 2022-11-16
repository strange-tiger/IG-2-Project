using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using Photon.Realtime;

using _UI = Defines.EPrivateRoomUIIndex;
using _DB = Asset.MySql.MySqlSetting;

public class MakeRoomUI : MonoBehaviourPunCallbacks
{
    [Header("Manager")]
    [SerializeField] PrivateRoomUIManager _uiManager;

    [Header("Button")]
    [SerializeField] Button _makeRoomButton;
    [SerializeField] Button _closeButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _roomNameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _roomNumberInput;

    [Space(15)]
    [SerializeField] Toggle _passwordToggle;

    private RoomOptions _roomOptions = new RoomOptions();
    private string _userId;

    private void Awake()
    {
        _roomOptions.IsVisible = true;
        _roomOptions.IsOpen = true;
        _roomOptions.PublishUserId = true;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _makeRoomButton.onClick.RemoveListener(RequestMakeRoom);
        _makeRoomButton.onClick.AddListener(RequestMakeRoom);

        _closeButton.onClick.RemoveListener(Close);
        _closeButton.onClick.AddListener(Close);

        _passwordToggle.isOn = false;
        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
        _passwordToggle.onValueChanged.AddListener(ActivePasswordInput);
    }

    public void RequestMakeRoom()
    {
        try
        {
            _userId = PhotonNetwork.LocalPlayer.UserId;

            MakeRoom();
        }
        catch
        {
            Debug.LogError("�� ���� ��û ����");
        }
    }

    private void MakeRoom()
    {
        try
        {
            PhotonNetwork.LeaveRoom();
        }
        catch
        {
            Debug.LogError("�κ� ���� ����");
        }

        Debug.Log("�� ���� �õ�");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("�Ƴ�?");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ����");
        CreatePrivateRoom();
    }

    private void CreatePrivateRoom()
    {
        string roomName = _userId + "_" + _passwordInput.text;

        try
        {
            _roomOptions.MaxPlayers = byte.Parse(_roomNumberInput.text);

            _roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { "roomname", _userId },
                { "password", _passwordInput.text },
                { "displayname", _roomNameInput.text }
            };
            _roomOptions.CustomRoomPropertiesForLobby = new string[]
            {
                "roomname",
                "password",
                "displayname"
            };

            _DB.AddNewRoomInfo(roomName, _passwordInput.text, _roomNameInput.text, int.Parse(_roomNumberInput.text));
            PhotonNetwork.CreateRoom(roomName, _roomOptions, null);
            Debug.Log("�� ���� ����");
        }
        catch
        {
            Debug.LogError("�� ���� ����");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("�� ����");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // �ӽ� �ڵ�
        Debug.Log("�� ����");
        PhotonNetwork.LoadLevel("PrivateRoom");
    }

    private void ActivePasswordInput(bool isOn)
    {
        _passwordInput.interactable = isOn;
        Debug.Log(isOn);
        if(!_passwordInput.IsInteractable())
        {
            Debug.Log(_passwordInput.text);
            _passwordInput.text = "";
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Close() => _uiManager.LoadUI(_UI.JOIN);

    public override void OnDisable()
    {
        base.OnDisable();

        _roomNameInput.text = "";
        _passwordInput.text = "";
        _roomNumberInput.text = "";

        _makeRoomButton.onClick.RemoveListener(RequestMakeRoom);
        _closeButton.onClick.RemoveListener(Close);
        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
    }
}

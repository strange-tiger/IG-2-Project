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
            Debug.LogError("방 생성 요청 실패");
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
            Debug.LogError("로비 입장 실패");
        }

        Debug.Log("방 생성 시도");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("됐나?");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 입장");
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
            Debug.Log("방 생성 성공");
        }
        catch
        {
            Debug.LogError("방 생성 실패");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("방 생성");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        // 임시 코드
        Debug.Log("방 입장");
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

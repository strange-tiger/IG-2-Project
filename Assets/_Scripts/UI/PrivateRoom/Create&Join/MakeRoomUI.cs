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

        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
        _passwordToggle.onValueChanged.AddListener(ActivePasswordInput);
        _passwordToggle.isOn = false;
    }

    /// <summary>
    /// 방 생성을 요청한다.
    /// MakeRoom을 호출한다.
    /// </summary>
    public void RequestMakeRoom()
    {
        try
        {
            MakeRoom();
        }
        catch
        {
            Debug.LogError("방 생성 요청 실패");
        }
    }

    /// <summary>
    /// 현재 방에서 나간다. 방을 생성하기 위해 마스터 서버로 나가야 한다.
    /// </summary>
    private void MakeRoom()
    {
        try
        {
            PhotonNetwork.LeaveRoom();
        }
        catch
        {
            Debug.LogError("방 퇴장 실패");
        }

        Debug.Log("방 생성 시도");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("마스터 서버 입장");
    }

    /// <summary>
    /// 성공적으로 마스터 서버의 로비까지 나간다면 CreatePrivateRoom을 호출한다.
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 입장");
        CreatePrivateRoom();
    }

    private string _roomName;
    /// <summary>
    /// 입력된 방 정보에 따라 방을 생성한다.
    /// _roomOptions에 방 정보를 할당하고 DB에 새로운 방의 정보를 저장한다.
    /// PhotonNetwork.CreateRoom을 호출한다.
    /// </summary>
    private void CreatePrivateRoom()
    {
        _roomName = PhotonNetwork.LocalPlayer.UserId + "_" + _passwordInput.text;

        if (_roomNameInput.text != string.Empty)
        {
            _roomOptions.MaxPlayers = byte.Parse(_roomNumberInput.text);
        }
        else
        {
            _roomOptions.MaxPlayers = 0;
        }

        _roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "roomname", _roomName },
            { "password", _passwordInput.text },
            { "displayname", _roomNameInput.text }
        };
        _roomOptions.CustomRoomPropertiesForLobby = new string[]
        {
            "roomname",
            "password",
            "displayname"
        };

        _DB.AddNewRoomInfo(_roomName, _passwordInput.text, _roomNameInput.text, int.Parse(_roomNumberInput.text));
        PhotonNetwork.CreateRoom(_roomName, _roomOptions, null);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    private const string PREV_SCENE = "PrevScene";
    /// <summary>
    /// FadeOut을 호출한다.
    /// PlayerPrefs로 레지스토리에 이전 씬 넘버를 저장하고 사설 공간 씬을 로드한다.
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        OVRScreenFade.instance.FadeOut();

        PlayerPrefs.SetInt(PREV_SCENE, SceneManagerHelper.ActiveSceneBuildIndex);
        PhotonNetwork.LoadLevel((int)Defines.ESceneNumber.PrivateRoom);
    }

    /// <summary>
    /// 비밀번호 체크박스 입력에 따라 비밀번호 입력 InputField 활성화 여부를 결정한다.
    /// </summary>
    /// <param name="isOn"></param>
    private void ActivePasswordInput(bool isOn)
    {
        _passwordInput.interactable = isOn;
        
        if(!_passwordInput.IsInteractable())
        {
            Debug.Log(_passwordInput.text);
            _passwordInput.text = string.Empty;
        }

        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 현재 창을 닫고 방 참가 창을 띄운다.
    /// </summary>
    private void Close() => _uiManager.LoadUI(_UI.JOIN);

    public override void OnDisable()
    {
        base.OnDisable();

        _roomNameInput.text = string.Empty;
        _passwordInput.text = string.Empty;
        _roomNumberInput.text = string.Empty;

        _makeRoomButton.onClick.RemoveListener(RequestMakeRoom);
        _closeButton.onClick.RemoveListener(Close);
        _passwordToggle.onValueChanged.RemoveListener(ActivePasswordInput);
    }
}

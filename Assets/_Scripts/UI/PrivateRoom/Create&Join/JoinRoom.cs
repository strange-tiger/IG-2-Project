using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _PH = ExitGames.Client.Photon;
using _DB = Asset.MySql.MySqlSetting;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    private static _PH.Hashtable _currentJoinRoom = new _PH.Hashtable();
    private static readonly _PH.Hashtable CUSTOM_ROOM_PROPERTIES_UNLOCKED =
        new _PH.Hashtable() { { "password", "" } };
    private const int ANY_MAX_PLAYER = 0;
    private const int DEFAULT_MAX_PLAYER = 8;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 랜덤 매칭을 실행한다. JoinRoomUi.RandomJoin에서 호출된다.
    /// _currentJoinRoom을 디폴트 값으로 할당하고 
    /// PhotonNetwork.LeaveRoom을 호출해 현재 룸에서 나가 마스터 서버에 들어간다.
    /// </summary>
    public static void JoinRandom()
    {
        _currentJoinRoom = CUSTOM_ROOM_PROPERTIES_UNLOCKED;
        PhotonNetwork.LeaveRoom();
    }

    /// <summary>
    /// 특정 방을 선택하여 입장할 때 사용한다.
    /// _currentJoinRoom에 roomInfo를 할당하고 
    /// PhotonNetwork.LeaveRoom을 호출해 현재 룸에서 나가 마스터 서버에 들어간다.
    /// </summary>
    /// <param name="roomInfo"></param>
    public static void JoinInRoom(_PH.Hashtable roomInfo)
    {
        _currentJoinRoom = roomInfo;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// 마스터 서버의 로비에 성공적으로 접속하면 호출된다.
    /// _currentJoinRoom의 정보에 따라 PhotonNetwork.JoinRandomOrCreateRoom을 호출한다.
    /// 해당하는 방이 있다면 입장하고 없다면 새로 만든다.
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        
        PhotonNetwork.JoinRandomOrCreateRoom(_currentJoinRoom, DEFAULT_MAX_PLAYER);
    }

    /// <summary>
    /// 방을 생성하면 호출된다. DB에 새로운 방 정보를 저장한다.
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        _DB.AddNewRoomInfo
        (
            _currentJoinRoom["roomname"].ToString(),
            _currentJoinRoom["password"].ToString(),
            _currentJoinRoom["displayname"].ToString(),
            DEFAULT_MAX_PLAYER
        );
    }

    /// <summary>
    /// 방에 입장하면 호출된다. FadeOut을 호출한다.
    /// FadeOut 이후 LoadAfterFadeOut을 실행한다.
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        OVRScreenFade.instance.FadeOut();

        StartCoroutine(LoadAfterFadeOut());
    }

    private static readonly WaitForSeconds FADE_DELAY = new WaitForSeconds(2f);
    /// <summary>
    /// FADE_DELAY의 지연 이후 (FadeOut 이후) 입장 이전 씬 번호를 PlayerPrefs로 레지스토리에 저장한다.
    /// 씬을 로드한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadAfterFadeOut()
    {
        yield return FADE_DELAY;

        PlayerPrefs.SetInt("PrevScene", SceneManagerHelper.ActiveSceneBuildIndex);
        PhotonNetwork.LoadLevel((int)Defines.ESceneNumber.PrivateRoom);
    }

}

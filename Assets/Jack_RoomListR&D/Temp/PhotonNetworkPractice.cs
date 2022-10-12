using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkPractice : MonoBehaviourPunCallbacks
{
    [Header("Info Text")]
    [SerializeField] private Text _infoText;

    [Header("Buttons")]
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _resetRoomListButton;
    [SerializeField] private Button _exitRoomButton;

    [Header("Room List")]
    [SerializeField] private GameObject _roomPanel;
    [SerializeField] private GameObject _scrollViewContent;

    private RoomOptions _roomOption = new RoomOptions()
    {
        MaxPlayers = 5
    };

    // 여기에 RoomList가 저장됨.
    private Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    private int _roomCount = 0;

    /* 서버/로비 접속 관련 함수 */
    private void Awake()
    {
        ResetButton();

        _infoText.text = "Connecting...";
        
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        _infoText.text = "Connected! Joinning Lobby...";
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        _infoText.text = "Lobby Joined!";
        SetInteractableOfAllButtons(true);
    }
    public override void OnLeftLobby()
    {
        _infoText.text = "Reconnecting Lobby...";
        SetInteractableOfAllButtons(false);
        PhotonNetwork.JoinLobby();
    }


    /// <summary>
    /// 방 리스트가 업데이트 될 때마다 호출되는 함수.
    /// 로비에 있을 때만 실행된다.
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _infoText.text = "Room List Updated";
        RoomListUpdate(roomList);
        Invoke("setInfoText", 0.5f);
    }
    /// <summary>
    /// cachedRoomList를 초기화 함
    /// </summary>
    /// <param name="roomList">OnRoomListUpdate에서 받아온 roomList</param>
    private void RoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log(roomList.Count);

        foreach (RoomInfo room in roomList)
        {
            // 방 리스트에 있는 방들 중 이미 삭제 된 방들은 cachedRoomList에서 제외된다.
            if(room.RemovedFromList)
            {
                cachedRoomList.Remove(room.Name);
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }

        SetRoomList();
    }
    /// <summary>
    /// cachedRoomList를 기반으로 UI를 보여줌.
    /// 효율적이지 못한 스크립트... 발전 필요
    /// </summary>
    private void SetRoomList()
    {
        // Room List Panel 초기화
        foreach(Text roomPanel in _scrollViewContent.GetComponentsInChildren<Text>())
        {
            Destroy(roomPanel.transform.parent.gameObject);
        }

        // cachedRoomList에 맞춰 Panel 설정
        foreach(KeyValuePair<string, RoomInfo> room in cachedRoomList)
        {
            // 해당 방이 이미 삭제되었다면 ui에서 제외함
            if(room.Value.RemovedFromList)
            {
                continue;
            }

            GameObject newRoomPanel = Instantiate(_roomPanel, _scrollViewContent.transform);
            newRoomPanel.GetComponentInChildren<Text>().text = room.Key;
        }

        _roomCount = cachedRoomList.Count;
    }
    /// <summary>
    /// 방 새로 고침
    /// </summary>
    private void ResetRoomList()
    {
        SetInteractableOfAllButtons(false);
        _infoText.text = "Resetting Room List...";

        SetRoomList();

        SetInteractableOfAllButtons(true);
        _infoText.text = "Done!";
        Invoke("setInfoText", 0.5f);
    }



    /* 방만들기 관련 함수 */
    private void CreateRoom()
    {
        ++_roomCount;
        PhotonNetwork.CreateRoom(_roomCount.ToString(), _roomOption, null, null);
        _infoText.text = "Creating Room...";
        SetRoomList();
    }
    public override void OnCreatedRoom()
    {
        _infoText.text = $"Created Room {PhotonNetwork.CurrentRoom.Name}";
        
        _createRoomButton.gameObject.SetActive(false);
        _resetRoomListButton.gameObject.SetActive(false);
        _exitRoomButton.gameObject.SetActive(true);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _infoText.text = $"Fail to Created Room {_roomCount}";
    }

    /* 기타 함수, 중요하지 않음 */
    private void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        _createRoomButton.gameObject.SetActive(true);
        _resetRoomListButton.gameObject.SetActive(true);
        _exitRoomButton.gameObject.SetActive(false);
    }
    private void ResetButton()
    {
        _createRoomButton.onClick.AddListener(() => { CreateRoom(); });
        _resetRoomListButton.onClick.AddListener(() => { ResetRoomList(); });
        _exitRoomButton.onClick.AddListener(() => { ExitRoom(); });

        SetInteractableOfAllButtons(false);

        _exitRoomButton.gameObject.SetActive(false);
    }
    private void SetInteractableOfAllButtons(bool value)
    {
        _createRoomButton.interactable = value;
        _resetRoomListButton.interactable = value;
        _exitRoomButton.interactable = value;
    }
    private void ResetInfoText()
    {
        _infoText.text = "Waiting...";
    }
}

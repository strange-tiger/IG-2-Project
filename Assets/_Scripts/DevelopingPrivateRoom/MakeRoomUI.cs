using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Oculus.Platform;
using Photon.Pun;
using UnityEditor.XR;

public class MakeRoomUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] Button _makeRoomButton;

    [Header("Input Field")]
    [SerializeField] TMP_InputField _roomNameInput;
    [SerializeField] TMP_InputField _passwordInput;
    [SerializeField] TMP_InputField _roomNumberInput;

    Photon.Realtime.RoomOptions _roomOptions;

    private string _userId;

    private void Awake()
    {
        _roomOptions.IsVisible = true;
        _roomOptions.IsOpen = true;
    }

    private void OnEnable()
    {

    }

    private void MakeRoom()
    {
        string roomName = _userId + "_" + _passwordInput.text;

        _roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "roomname", _userId},
            {"password", _passwordInput.text },
            {"displayname", _roomNameInput.text }
        };
        _roomOptions.CustomRoomPropertiesForLobby = new string[] {
            "roomname",
            "password",
            "displayname"
        };

        PhotonNetwork.CreateRoom(roomName, _roomOptions, null);
    }

    private void OnDisable()
    {
        
    }
}

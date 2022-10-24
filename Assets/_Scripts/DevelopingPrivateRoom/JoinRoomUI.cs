using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomUI : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] PrivateRoomUIManager _uiManager;

    [Header("Button")]
    [SerializeField] Button _makeRoomButton;
    [SerializeField] Button _refreshButton;
    [SerializeField] Button _randomJoinButton;

    [Header("Text")]
    [SerializeField] TextMeshProUGUI[] _roomInfoText;
}

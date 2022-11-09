using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChange : ServerChange
{
    [SerializeField] private string _nextLobbyName;

    protected override string CheckMessage()
    {
        return _nextLobbyName + "로 이동하시겠습니까?";
    }
}

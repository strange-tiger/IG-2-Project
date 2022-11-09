using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChange : ServerChange
{
    [SerializeField] private string _nextLobbyName;

    protected override string CheckMessage()
    {
        return _nextLobbyName + "�� �̵��Ͻðڽ��ϱ�?";
    }
}

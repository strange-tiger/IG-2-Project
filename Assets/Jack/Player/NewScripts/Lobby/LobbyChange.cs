using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChange : InteracterableObject
{
    [SerializeField] private Defines.ESceneNumder _lobbyType;
    [SerializeField] private LobbyChanger _lobbyManager;

    public override void Interact()
    {
        base.Interact();
        _lobbyManager.ChangeLobby(_lobbyType);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyChange : InteracterableObject
{
    [SerializeField] private string _nextLobbyName;
    [SerializeField] private Defines.ESceneNumder _lobbyType;
    [SerializeField] private LobbyChanger _lobbyManager;

    public override void Interact()
    {
        base.Interact();
        MenuUIManager.Instance.ShowCheckPanel(_nextLobbyName + "로 이동하시겠습니까?",
            () => { _lobbyManager.ChangeLobby(_lobbyType); },
            () => { });
    }
}

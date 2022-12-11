using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumber;

public class ServerChange : InteracterableObject
{
    [SerializeField] protected SceneNumber _sceneType;
    [SerializeField] protected LobbyChanger _lobbyManager;
    [SerializeField] protected string _checkMessage;

    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel(CheckMessage(),
            () => { ChangeLobby(); },
            () => { });
    }

    protected virtual void ChangeLobby()
    {
        _lobbyManager.ChangeLobby(_sceneType);
    }

    protected virtual string CheckMessage()
    {
        return _checkMessage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SceneNumber = Defines.ESceneNumber;

public class ArenaStartNPC : InteracterableObject
{
    [SerializeField] private LobbyChanger _lobbyChanger;
    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel("Go Arena?",
            () => {
                _lobbyChanger.ChangeLobby(SceneNumber.ArenaRoom);
            },
            () => { }
            );
    }
}

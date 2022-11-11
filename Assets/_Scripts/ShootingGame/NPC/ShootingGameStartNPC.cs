using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using SceneNumber = Defines.ESceneNumder;

public class ShootingGameStartNPC : InteracterableObject
{
    [SerializeField] private LobbyChanger _lobbyChanger;
    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel("Play?",
            () => {
                _lobbyChanger.ChangeLobby(SceneNumber.ShootingWaitingRoom);
            },
            () => { }
            );
    }
}

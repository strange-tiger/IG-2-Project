using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SceneNumber = Defines.ESceneNumder;

public class ShootingGameStartNPC : InteracterableObject
{
    [SerializeField] private LobbyChanger _lobbyChanger;
    private RoomOptions _waitingRoomOption = new RoomOptions
    {
        MaxPlayers = ShootingGameManager._MAX_PLAYER_COUNT,
        CleanupCacheOnLeave = true,
        PublishUserId = true,
    };

    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel("Play?",
            () => {
                _lobbyChanger.ChangeLobby(SceneNumber.ShootingWaitingRoom, _waitingRoomOption, true);
            },
            () => { }
            );
    }
}

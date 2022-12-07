using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using SceneNumber = Defines.ESceneNumber;

public class ShootingGameStartNPC : InteracterableObject
{
    [SerializeField] private LobbyChanger _lobbyChanger;
    private RoomOptions _waitingRoomOption = new RoomOptions
    {
        MaxPlayers = ShootingGameManager._MAX_PLAYER_COUNT,
        CleanupCacheOnLeave = true,
        PublishUserId = true,
        CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { ShootingServerManager.RoomPropertyKey, 1 } },
        CustomRoomPropertiesForLobby = new string[]
                {
                    ShootingServerManager.RoomPropertyKey,
                },
        IsVisible = true,
        IsOpen = true,
    };
    [SerializeField] private string _checkMessage = "총쏘기 게임에 참여하시겠습니까?";

    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel(_checkMessage,
            () => {
                _lobbyChanger.ChangeLobby(SceneNumber.ShootingWaitingRoom, _waitingRoomOption, true,
                    _waitingRoomOption.CustomRoomProperties, (byte) _waitingRoomOption.MaxPlayers);
            },
            () => { }
            );
    }
}

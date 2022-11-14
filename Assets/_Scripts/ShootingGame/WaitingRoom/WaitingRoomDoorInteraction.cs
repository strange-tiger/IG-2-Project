using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumder;

public class WaitingRoomDoorInteraction : InteracterableObject
{
    [SerializeField] private string _exitComfirmMessage = "�����ðڽ��ϱ�?";
    [SerializeField] private LobbyChanger _lobbyChanger;

    public override void Interact()
    {
        MenuUIManager.Instance.ShowCheckPanel(_exitComfirmMessage,
            () => {
                _lobbyChanger.ChangeLobby(SceneNumber.WesternLobby);
            },
            () => { }
            );
    }
}

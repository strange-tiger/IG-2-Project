using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumber;

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

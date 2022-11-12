using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumder;

public class ExitWaiting : InteracterableObject
{
    [SerializeField] private string _exitComfirmMessage = "�����ðڽ��ϱ�?";
    [SerializeField] private LobbyChanger _lobbyChanger;

    public void OnEnable()
    {
        this.enabled = false;
    }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumber;

public class PrivateRoomExit : ServerChange
{
    [Header("Destroy Room")]
    [SerializeField] RoomDestroyer _roomDestroyer;

    private const string PREV_SCENE = "PrevScene";
    public override void Interact()
    {
        Debug.Log(gameObject.name + ": interact");

        if (PlayerPrefs.HasKey(PREV_SCENE))
        {
            _sceneType = (SceneNumber)PlayerPrefs.GetInt(PREV_SCENE);
            PlayerPrefs.DeleteKey(PREV_SCENE);
        }

        MenuUIManager.Instance.ShowCheckPanel(CheckMessage(),
            () => { 
                Destroy(_roomDestroyer);
                _lobbyManager.ChangeLobby(_sceneType);
            },
            () => { });
    }
}

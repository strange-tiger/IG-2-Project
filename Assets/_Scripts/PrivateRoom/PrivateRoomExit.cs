using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumber;

public class PrivateRoomExit : ServerChange
{
    [Header("Destroy Room")]
    [SerializeField] RoomDestroyer _roomDestroyer;

    private const string PREV_SCENE = "PrevScene";
    /// <summary>
    /// 방에서 나갈 때, 방에 들어오기 전 있었던 방(씬)으로 돌아가도록 한다.
    /// 레지스토리의 PREV_SCENE 위치에 저장되어있는 씬 넘버를 읽어 _sceneType에 할당한다.
    /// MenuUIManager의 ShowCheckPanel을 호출하면서 _sceneType을 전달한다.
    /// </summary>
    public override void Interact()
    {
        if (PlayerPrefs.HasKey(PREV_SCENE))
        {
            _sceneType = (SceneNumber)PlayerPrefs.GetInt(PREV_SCENE);
            PlayerPrefs.DeleteKey(PREV_SCENE);
        }

        MenuUIManager.Instance.ShowCheckPanel
        (
            CheckMessage(),
            () => { 
                Destroy(_roomDestroyer);
                _lobbyManager.ChangeLobby(_sceneType);
            },
            () => { }
        );
    }
}

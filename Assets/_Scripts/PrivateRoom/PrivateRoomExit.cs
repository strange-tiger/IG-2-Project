using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumder;

public class PrivateRoomExit : ServerChange
{
    private const string PREV_SCENE = "PrevScene";
    public override void Interact()
    {
        if (PlayerPrefs.HasKey(PREV_SCENE))
        {
            _sceneType = (SceneNumber)PlayerPrefs.GetInt(PREV_SCENE);
            PlayerPrefs.DeleteKey(PREV_SCENE);
        }
        base.Interact();
    }
}

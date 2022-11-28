using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Asset.MySql;
using SceneNumber = Defines.ESceneNumder;
using Photon.Pun;

public class LobbyChange : ServerChange
{
    [SerializeField] private string _nextLobbyName;

    public override void Interact()
    {
        if(SceneNumber.ArenaRoom == _sceneType)
        {
            if(!MySqlSetting.CheckCompleteTutorial(PhotonNetwork.NickName,ETutorialCompleteState.ARENA))
            {
                _sceneType = SceneNumber.ArenaTutorialRoom;
            }
        }
        base.Interact();

    }

    protected override string CheckMessage()
    {
        return _nextLobbyName + "로 이동하시겠습니까?";
    }
}

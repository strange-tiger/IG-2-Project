using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Asset.MySql;
using SceneNumber = Defines.ESceneNumder;
using Photon.Pun;

public class LobbyChange : ServerChange
{
    [SerializeField] private string _nextLobbyName;
    [SerializeField] private bool _isTutorialExist;
    [SerializeField] private SceneNumber _tutorialSceneNumber;
    [SerializeField] private ETutorialCompleteState _state;


    public override void Interact()
    {
        if (_isTutorialExist)
        {
            TutorialCheck();
        }

        base.Interact();

    }

    protected override string CheckMessage()
    {
        return _nextLobbyName + "�� �̵��Ͻðڽ��ϱ�?";
    }

    private void TutorialCheck()
    {

        if (!MySqlSetting.CheckCompleteTutorial(PhotonNetwork.NickName, _state))
        {
            _sceneType = _tutorialSceneNumber;
        }

    }
}

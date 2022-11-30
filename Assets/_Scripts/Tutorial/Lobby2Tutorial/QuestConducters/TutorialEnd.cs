using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneNumber = Defines.ESceneNumber;

public class TutorialEnd : QuestConducter
{
    [SerializeField] private SceneNumber _nextLobby = SceneNumber.WesternLobby;
    [SerializeField] private LobbyChanger _lobbyChanger;

    public override void StartQuest()
    {
        _lobbyChanger.ChangeLobby(_nextLobby);
        QuestEnded();
    }
}

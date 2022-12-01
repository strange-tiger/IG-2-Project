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

    protected override void OnQuestEnded()
    {
        // 퀘스트 끝 효과음 나오면 안됨
    }
}

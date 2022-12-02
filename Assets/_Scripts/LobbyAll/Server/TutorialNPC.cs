using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPC : ServerChange
{
    protected override void ChangeLobby()
    {
        _lobbyManager.ChangeLobby(_sceneType, true);
    }
}

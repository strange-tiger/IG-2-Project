using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lobby1TutorialObject : MonoBehaviour
{
    [SerializeField] private TutorialController _tutorialController;

    public virtual void SendQuestClearMessage()
    {
        _tutorialController.QuestClearEvent.Invoke(false);
    }
}

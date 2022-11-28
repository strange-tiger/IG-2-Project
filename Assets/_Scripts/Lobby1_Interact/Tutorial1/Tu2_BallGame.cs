using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tu2_BallGame : Lobby1TutorialObject
{
    [SerializeField] private TextMeshProUGUI _clearQuestNum;

    private int _advanceQuest;
    public int AdvanceQuest { get { return _advanceQuest; } set { _advanceQuest = value; } }

    void Update()
    {
        _clearQuestNum.text = $"{_advanceQuest}/7";

        if (_advanceQuest == 1)
        {
            SendQuestClearMessage();
        }
    }

    public override void SendQuestClearMessage()
    {
        base.SendQuestClearMessage();
    }
}

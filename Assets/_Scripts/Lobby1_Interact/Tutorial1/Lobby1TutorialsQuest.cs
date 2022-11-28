using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lobby1TutorialsQuest : Lobby1TutorialObject
{
    enum QuestNumber
    {
        Start,
        Tu1,
        Tu2,
        Tu3,
        Tu4,
        Tu5,
        Tu6,
        End,
    }

    [SerializeField] private TextMeshProUGUI _clearQuestNum;
    [SerializeField] private QuestNumber _questNumber;

    private bool _isQuest1;
    private int _advanceQuest;
    public int AdvanceQuest { get { return _advanceQuest; } set { _advanceQuest = value; } }

    private void OnEnable()
    {
        _advanceQuest = 0;
    }

    void Update()
    {
        if ((int)_questNumber == 1)
        {
            _clearQuestNum.text = $"{_advanceQuest}/7";

            if (_advanceQuest == 7)
            {
                SendQuestClearMessage();
            }
        }

        if ((int)_questNumber == 2)
        {
            _clearQuestNum.text = $"{_advanceQuest}/1";

            if (_advanceQuest == 1)
            {
                SendQuestClearMessage();
            }
        }

        if ((int)_questNumber == 3)
        {
            _clearQuestNum.text = $"{_advanceQuest}/1";

            if (_advanceQuest == 1)
            {
                SendQuestClearMessage();
            }
        }

        if ((int)_questNumber == 4)
        {
            _clearQuestNum.text = $"{_advanceQuest}/5";

            if (_advanceQuest == 5)
            {
                SendQuestClearMessage();
            }
        }

        if ((int)_questNumber == 5)
        {
            _clearQuestNum.text = $"{_advanceQuest}/1";

            if (_advanceQuest == 1)
            {
                SendQuestClearMessage();
            }
        }

        if ((int)_questNumber == 6)
        {
            if (!_isQuest1)
            {
                _clearQuestNum.text = $"{_advanceQuest}/1";

                if (_advanceQuest == 1)
                {
                    SendQuestClearMessage();
                    _advanceQuest = 0;
                    _isQuest1 = true;
                }
            }
            else
            {
                _clearQuestNum.text = $"{_advanceQuest}/4";

                if (_advanceQuest == 4)
                {
                    SendQuestClearMessage();
                }
            }
        }
    }

    public override void SendQuestClearMessage()
    {
        base.SendQuestClearMessage();
    }
}

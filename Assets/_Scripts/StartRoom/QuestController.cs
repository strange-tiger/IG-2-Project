using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questText;

    [SerializeField] private StartRoomTutorial _startRoomTutorial;

    void Update()
    {
        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Run)
        {
            if (_startRoomTutorial.DialogueNum == 4)
            {
                _questText.text = "달리기 기능 3초 유지 시 다음으로 넘어감";
            }
            else
            {
                _questText.text = null;
            }
        }

        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Grabber)
        {
            if (_startRoomTutorial.DialogueNum == 2)
            {
                _questText.text = "집가고싶다";
            }
            else
            {
                _questText.text = null;
            }
        }

        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Ray)
        {
            if (_startRoomTutorial.DialogueNum == 2)
            {
                _questText.text = "자고싶다";
            }
            else
            {
                _questText.text = null;
            }
        }
    }
}


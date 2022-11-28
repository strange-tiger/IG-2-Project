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
            if (_startRoomTutorial.IsTutorialQuest)
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
            if (_startRoomTutorial.DialogueNum == 1)
            {
                _questText.text = "그랩 해 보세요";
            }
            else
            {
                _questText.text = null;
            }
        }

        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Ray)
        {
            if (_startRoomTutorial.DialogueNum == 1)
            {
                _questText.text = "레이캐스트를 이용 해 그랩 해 보세요";
            }
            else
            {
                _questText.text = null;
            }
        }
    }
}
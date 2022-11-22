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
                _questText.text = "�޸��� ��� 3�� ���� �� �������� �Ѿ";
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
                _questText.text = "������ʹ�";
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
                _questText.text = "�ڰ�ʹ�";
            }
            else
            {
                _questText.text = null;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _questText;

    [SerializeField] private StartRoomTutorial _startRoomTutorial;

    [SerializeField] private TextMeshProUGUI _startRoomNPCName;

    private void Start()
    {
        _startRoomNPCName.text = "����";
    }

    void Update()
    {
        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Run)
        {
            if (_startRoomTutorial.IsTutorialQuest)
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
            if (_startRoomTutorial.IsTutorialQuest)
            {
                _questText.text = "�׷� �� ������";
            }
            else
            {
                _questText.text = null;
            }
        }
           
        if (_startRoomTutorial.TurtorialTypeNum == (int)StartRoomTutorial.TurtorialType.Ray)
        {
            if (_startRoomTutorial.IsTutorialQuest)
            {
                _questText.text = "����ĳ��Ʈ�� �̿� �� �׷� �� ������";
            }
            else
            {
                _questText.text = null;
            }
        }
    }
}


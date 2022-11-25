using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _CSV = Asset.ParseCSV.CSVParser;

public class QuestController : MonoBehaviour
{
    public enum TutorialType
    {
        StartRoom,
        Lobby1,
    }

    
    [SerializeField] private StartRoomTutorial _startRoomTutorial;
    [SerializeField] private TutorialController _tutorialController;
    

    [SerializeField] private TutorialType _tutorialType;

    private List<string> _startRoomTutorialCSV = new List<string>();

    void Update()
    {
        if (_tutorialType == TutorialType.StartRoom)
        {
            
            

            
            
            
        }

        if (_tutorialType == TutorialType.Lobby1)
        {

        }
    }
}


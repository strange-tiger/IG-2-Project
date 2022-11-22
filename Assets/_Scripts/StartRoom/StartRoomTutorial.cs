using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _CSV = Asset.ParseCSV.CSVParser;

public class StartRoomTutorial : MonoBehaviour
{
    private List<string> _tutorialRunList = new List<string>();
    private List<string> _tutorialGrabberList = new List<string>();
    private List<string> _tutorialRayList = new List<string>();

    void Start()
    {
        _CSV.ParseCSV("StartRoomTutorialRun", _tutorialRunList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialRun", _tutorialGrabberList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialRun", _tutorialRayList, '\n', ',');


    }

    void Update()
    {

    }
}

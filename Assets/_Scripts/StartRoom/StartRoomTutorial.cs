using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using _CSV = Asset.ParseCSV.CSVParser;

public class StartRoomTutorial : MonoBehaviour
{
    private List<string> _tutorialRunList = new List<string>();
    private List<string> _tutorialGrabberList = new List<string>();
    private List<string> _tutorialRayList = new List<string>();

    enum TurtorialType
    {
        Run,
        Grabber,
        Ray,
        End,
    }

    [SerializeField] private TextMeshProUGUI _tutorialRunText;
    [SerializeField] private TurtorialType _turtorialType;

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    private bool _isDialogueEnd;
    private int _dialogueNum;
    private int _dialogueMaxNum;

    void Start()
    {
        _CSV.ParseCSV("StartRoomTutorialRun", _tutorialRunList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialGrabber", _tutorialGrabberList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialRay", _tutorialRayList, '\n', ',');

        StartCoroutine(TextTyping(_tutorialRunList[_tutorialRunList.Count - _tutorialRunList.Count]));
    }

    private void Update()
    {
        if (_isDialogueEnd)
        {
            
        }
    }

    IEnumerator TextTyping(string dialogue)
    {
        _isDialogueEnd = false;

        foreach (char c in dialogue)
        {
            _tutorialRunText.text += c;

            yield return _delayTime;

            if (Input.GetKeyDown(KeyCode.K))
            {
                _tutorialRunText.text = dialogue;

                StopCoroutine(TextTyping(dialogue));

                _isDialogueEnd = true;

                yield break;
            }
        }

        _isDialogueEnd = true;
    }

    private int DialogueNumCount()
    {
        if (_turtorialType == TurtorialType.Run)
        {
            _dialogueMaxNum = _tutorialRunList.Count;
        }

        else if (_turtorialType == TurtorialType.Grabber)
        {
            _dialogueMaxNum = _tutorialGrabberList.Count;
        }

        else if (_turtorialType == TurtorialType.Ray)
        {
            _dialogueMaxNum = _tutorialRayList.Count;
        }

        return _dialogueNum;
    }
}

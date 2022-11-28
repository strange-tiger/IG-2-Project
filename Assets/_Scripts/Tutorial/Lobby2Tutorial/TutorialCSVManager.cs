using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.ParseCSV;
using TutorialNumber = Defines.ELobby2TutorialNumber;

public class TutorialCSVManager : MonoBehaviour
{
    public class TutorialField
    {
        public const string TutorialID = "TutorialID";
        public const string StartID = "StartID";

        public const string ID = "ID";
        public const string Name = "Name";
        public const string Dialogue = "Dialogue";
        public const string Next = "Next";
        public const string IsQuest = "IsQuest";
        public const string Notes = "Notes";
    }

    [SerializeField] private TextAsset _tutorialSettingCSVFile;
    [SerializeField] private TextAsset _dialogueCSVFile;

    private char _lineSeparater = '\n';
    [SerializeField] private char _fieldSeparater = '@';

    private List<int> _settingInfos = new List<int>();
    private List<Dictionary<string, string>> _dialogues = new List<Dictionary<string, string>>();

    private void Awake()
    {
        Debug.Log("[Tutorial] CSV Awake");
        // 0. 튜토리얼 세팅 파일 받아오기
        List<Dictionary<string, string>> tempSettingInfos = 
            CSVParser.ParseCSV(_tutorialSettingCSVFile.name, _lineSeparater, _fieldSeparater);
        foreach(Dictionary<string, string> info in tempSettingInfos)
        {
            _settingInfos.Add(int.Parse(info[TutorialField.StartID]));
        }

        // 1. 다이얼 로그 파일 받아오기
        _dialogues = CSVParser.ParseCSV(_dialogueCSVFile.name, _lineSeparater, _fieldSeparater);
    }

    public int GetTutorialStartPoint(TutorialNumber number)
    {
        return _settingInfos[(int)number];
    }

    public Dictionary<string, string> GetDialogue(int dialogueId)
    {
        return _dialogues[dialogueId - 1];
    }
}

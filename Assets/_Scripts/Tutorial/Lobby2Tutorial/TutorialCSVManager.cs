using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.ParseCSV;
using TutorialNumber = Defines.ELobby2TutorialNumber;

/// <summary>
/// 튜토리얼을 위한 CSV를 미리 파싱하여 저장해둠
/// </summary>
public class TutorialCSVManager : MonoBehaviour
{
    /// <summary>
    /// 튜토리얼 필드명
    /// </summary>
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

    /// <summary>
    /// 튜토리얼과 관련된 CSV 파일
    /// </summary>
    [SerializeField] private TextAsset _tutorialSettingCSVFile;
    [SerializeField] private TextAsset _dialogueCSVFile;

    /// <summary>
    /// CSV 파싱을 위한 문자
    /// </summary>
    private char _lineSeparater = '\n';
    [SerializeField] private char _fieldSeparater = '@';

    /// <summary>
    /// 각 튜토리얼마다 시작 대화 번호
    /// </summary>
    private List<int> _settingInfos = new List<int>();
    /// <summary>
    /// 대화번호에 따른 대화 내용
    /// </summary>
    private List<Dictionary<string, string>> _dialogues = new List<Dictionary<string, string>>();

    private void Awake()
    {
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

    /// <summary>
    /// 해당 튜토리얼의 시작 대화 번호를 받아옴
    /// </summary>
    /// <param name="number">튜토리얼 번호</param>
    /// <returns>튜토리얼 시작 대화 번호</returns>
    public int GetTutorialStartPoint(TutorialNumber number)
    {
        return _settingInfos[(int)number];
    }

    /// <summary>
    /// 대화 번호를 받아 해당하는 대화 정보를 반환
    /// </summary>
    /// <param name="dialogueId">선택한 대화 번호</param>
    /// <returns>대화 정보</returns>
    public Dictionary<string, string> GetDialogue(int dialogueId)
    {
        return _dialogues[dialogueId - 1];
    }
}

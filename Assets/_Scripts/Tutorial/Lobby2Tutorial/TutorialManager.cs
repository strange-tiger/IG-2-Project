using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;
using Photon.Pun;
using TutorialState = Asset.MySql.ETutorialCompleteState;

/// <summary>
/// TutorialConducter와 UI, TutorialCSVManager를 연결해줌
/// </summary>
public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial")]
    // 튜토리얼 종류
    [SerializeField] private TutorialState _tutorialType;

    [Header ("UIs")]
    [Header("TutorialButtons")]
    // 튜토리얼이 있는 패널
    [SerializeField] private GameObject _tutorialPanel;
    // 나가기 안내 메시지
    [SerializeField] private string _tutorialEndCheckMessage = "나가시겠습니까?";

    [Header("DialoguePanel")]
    // 대사 출력 패널
    [SerializeField] private GameObject _DialoguePanel;
    [SerializeField] private TextMeshProUGUI _nameText;     // 이름 텍스트
    [SerializeField] private TextMeshProUGUI _dialogueText; // 대사 텍스트

    [Header("QuestPanel")]
    // 퀘스트 안내문이 들어가 있는 패널
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private TextMeshProUGUI _questText; // 퀘스트 텍스트

    [Header ("CSV")]
    // 튜토리얼에 필요한 CSV를 미리 받아 저장해두고 있는 Manager
    [SerializeField] private TutorialCSVManager _csvManager;
    public TutorialCSVManager CSVManager    // TutorialConducter에서 사용
    {
        get => _csvManager;
    }

    // 튜토리얼 목록
    private TutorialConducter[] _tutorialConducters;
    // 현재 진행중인 튜토리얼 목록(한번에 하나의 튜토리얼만 실행하기 위한 용도)
    private int _currentShowingTutorial = 0;

    private void Awake()
    {
        // 튜토리얼 완료 세팅
        MySqlSetting.CompleteTutorial(PhotonNetwork.NickName, _tutorialType);

        // 튜토리얼 실행자 세팅
        _tutorialConducters = GetComponentsInChildren<TutorialConducter>();
        foreach (TutorialConducter tutorial in _tutorialConducters)
        {
            tutorial.gameObject.SetActive(false);
        }

        // 버튼 세팅
        Button[] tutorialButtons = _tutorialPanel.GetComponentsInChildren<Button>();
        int buttonCount = tutorialButtons.Length - 1; // 마지막 나가기 버튼은 제외
        for (int i = 0; i < buttonCount; ++i)
        {
            int tutorialNumber = i + 1;
            tutorialButtons[i].onClick.RemoveAllListeners();
            tutorialButtons[i].onClick.AddListener(() => {
                ShowTutorial(tutorialNumber);
            });
        }

        // 나가기 버튼 기능 연결
        tutorialButtons[buttonCount].onClick.RemoveAllListeners();
        tutorialButtons[buttonCount].onClick.AddListener(() => {
            MenuUIManager.Instance.ShowCheckPanel(_tutorialEndCheckMessage,
                () => {
                    ShowTutorial(buttonCount + 1);
                },
                () => { });
        });

        _questPanel.SetActive(false);

        // 시작 튜토리얼을 실행시킴
        ShowTutorial(0);
    }

    private void Start()
    {
        // 시작 튜토리얼이 끝나기 전까지 튜토리얼 버튼 패널 꺼두기
        _tutorialPanel.SetActive(false);
    }

    /// <summary>
    /// 현재 실행중인 튜토리얼 중지 후, 선택한 튜토리얼 시작
    /// </summary>
    /// <param name="tutorialNumber">선택한 튜토리얼</param>
    private void ShowTutorial(int tutorialNumber)
    {
        _tutorialConducters[_currentShowingTutorial].gameObject.SetActive(false);
        _tutorialConducters[tutorialNumber].gameObject.SetActive(true);
        _currentShowingTutorial = tutorialNumber;
    }

    /// <summary>
    /// 대화 출력
    /// </summary>
    /// <param name="name">이름</param>
    /// <param name="dialogue">대화 내용</param>
    public void ShowDialogue(string name, string dialogue)
    {
        _nameText.text = name;
        _dialogueText.text = dialogue;
        _DialoguePanel.SetActive(true);
    }
    public void ShowDialogue(string dialogue)
    {
        _dialogueText.text = dialogue;
    }
    public void ShowDialogue()
    {
        _nameText.text = _dialogueText.text = "";
        _DialoguePanel.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 안내문 출력
    /// </summary>
    /// <param name="message">퀘스트 안내문</param>
    public void ShowQuestText(string message)
    {
        ShowDialogue();
        _questText.text = message;
        _questPanel.SetActive(true);
    }
    /// <summary>
    /// 퀘스트 패널 Disable
    /// </summary>
    public void DisableQuestText()
    {
        _questPanel.SetActive(false);
        _questText.text = "";
    }
}

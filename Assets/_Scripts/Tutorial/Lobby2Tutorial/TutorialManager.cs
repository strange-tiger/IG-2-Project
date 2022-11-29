using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Asset.MySql;
using Photon.Pun;
using TutorialState = Asset.MySql.ETutorialCompleteState;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private TutorialState _tutorialNumber;

    [Header ("UIs")]
    [Header("TutorialButtons")]
    [SerializeField] private GameObject _tutorialPanel;
    [SerializeField] private string _tutorialEndCheckMessage = "나가시겠습니까?";

    [Header("DialoguePanel")]
    [SerializeField] private GameObject _DialoguePanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [Header("QuestPanel")]
    [SerializeField] private GameObject _questPanel;
    [SerializeField] private TextMeshProUGUI _questText;

    [Header ("CSV")]
    [SerializeField] private TutorialCSVManager _csvManager;
    public TutorialCSVManager CSVManager
    {
        get => _csvManager;
    }

    private TutorialConducter[] _tutorialConducters;
    private int _currentShowingTutorial = 0;

    private void Awake()
    {
        // 튜토리얼 완료 안내
        //MySqlSetting.CompleteTutorial(PhotonNetwork.NickName, _tutorialNumber);
        Debug.Log("튜토리얼 완료");

        // 튜토리얼 세팅
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
            string name = tutorialButtons[i].name;

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
        _tutorialPanel.SetActive(false);
    }

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
    public void DisableQuestText()
    {
        _questPanel.SetActive(false);
        _questText.text = "";
    }
}

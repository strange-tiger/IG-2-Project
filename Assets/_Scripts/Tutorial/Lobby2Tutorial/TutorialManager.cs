using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header ("UIs")]
    [Header("TutorialButtons")]
    [SerializeField] private GameObject _tutorialPanel;

    [Header("DialogPanel")]
    [SerializeField] private GameObject _DialogPanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogText;

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
        // 튜토리얼 세팅
        _tutorialConducters = GetComponentsInChildren<TutorialConducter>();
        foreach (TutorialConducter tutorial in _tutorialConducters)
        {
            tutorial.gameObject.SetActive(false);
        }

        // 버튼 세팅
        Button[] tutorialButtons = _tutorialPanel.GetComponentsInChildren<Button>();
        int buttonCount = tutorialButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            int tutorialNumber = i + 1;
            tutorialButtons[i].onClick.RemoveAllListeners();
            tutorialButtons[i].onClick.AddListener(() => {
                ShowTutorial(tutorialNumber);
            });
        }

        _questPanel.SetActive(false);

        // 시작 튜토리얼을 실행시킴
        ShowTutorial(0);
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
    /// <param name="dialog">대화 내용</param>
    public void ShowDialog(string name, string dialog)
    {
        _nameText.text = name;
        _dialogText.text = dialog;
        _DialogPanel.SetActive(true);
    }
    public void ShowDialog(string dialog)
    {
        _dialogText.text = dialog;
    }
    public void ShowDialog()
    {
        _nameText.text = _dialogText.text = "";
        _DialogPanel.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 안내문 출력
    /// </summary>
    /// <param name="message">퀘스트 안내문</param>
    public void ShowQuestText(string message)
    {
        ShowDialog();
        _questText.text = message;
        _questPanel.SetActive(true);
    }
}

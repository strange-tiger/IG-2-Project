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
        // Ʃ�丮�� ����
        _tutorialConducters = GetComponentsInChildren<TutorialConducter>();
        foreach (TutorialConducter tutorial in _tutorialConducters)
        {
            tutorial.gameObject.SetActive(false);
        }

        // ��ư ����
        Button[] tutorialButtons = _tutorialPanel.GetComponentsInChildren<Button>();
        int buttonCount = tutorialButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            int tutorialNumber = i + 1;
            string name = tutorialButtons[i].name;

            tutorialButtons[i].onClick.RemoveAllListeners();
            tutorialButtons[i].onClick.AddListener(() => {
                ShowTutorial(tutorialNumber, name);
            });
        }

        _questPanel.SetActive(false);

        // ���� Ʃ�丮���� �����Ŵ
        ShowTutorial(0, "manager");
    }

    private void Start()
    {
        _tutorialPanel.SetActive(false);
    }

    private void ShowTutorial(int tutorialNumber, string debug)
    {
        Debug.Log("[Tutorial] ShowTutorial " + debug);
        _tutorialConducters[_currentShowingTutorial].gameObject.SetActive(false);
        _tutorialConducters[tutorialNumber].gameObject.SetActive(true);
        _currentShowingTutorial = tutorialNumber;
    }

    /// <summary>
    /// ��ȭ ���
    /// </summary>
    /// <param name="name">�̸�</param>
    /// <param name="dialogue">��ȭ ����</param>
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
    /// ����Ʈ �ȳ��� ���
    /// </summary>
    /// <param name="message">����Ʈ �ȳ���</param>
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

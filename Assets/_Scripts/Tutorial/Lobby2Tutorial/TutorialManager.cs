using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header ("UI")]
    [SerializeField] private GameObject _tutorialCanvas;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogText;
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
        Button[] tutorialButtons = _tutorialCanvas.GetComponentsInChildren<Button>();
        int buttonCount = tutorialButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            int tutorialNumber = i + 1;
            tutorialButtons[i].onClick.RemoveAllListeners();
            tutorialButtons[i].onClick.AddListener(() => {
                ShowTutorial(tutorialNumber);
            });
        }

        // ���� Ʃ�丮���� �����Ŵ
        ShowTutorial(0);
    }

    private void ShowTutorial(int tutorialNumber)
    {
        _tutorialConducters[_currentShowingTutorial].gameObject.SetActive(false);
        _tutorialConducters[tutorialNumber].gameObject.SetActive(true);
        _currentShowingTutorial = tutorialNumber;
    }

    /// <summary>
    /// ��ȭ ���
    /// </summary>
    /// <param name="name">�̸�</param>
    /// <param name="dialog">��ȭ ����</param>
    public void ShowDialog(string name, string dialog)
    {
        _nameText.text = name;
        _dialogText.text = dialog;
    }
    public void ShowDialog(string dialog)
    {
        _dialogText.text = dialog;
    }
    public void ShowDialog()
    {
        _nameText.text = _dialogText.text = "";
    }

    /// <summary>
    /// ����Ʈ �ȳ��� ���
    /// </summary>
    /// <param name="message">����Ʈ �ȳ���</param>
    public void ShowQuestText(string message)
    {
        ShowDialog();
        _questText.text = message;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialNumber = Defines.ELobby2TutorialNumber;
using TutorialField = TutorialCSVManager.TutorialField;

public class TutorialConducter : MonoBehaviour
{
    [SerializeField] private TutorialNumber _tutorialNumber;

    [Header ("Skip")]
    [SerializeField] private OVRInput.Button _skipButton;
    [SerializeField] private float _letterPassTime = 0.1f;

    private TutorialManager _tutorialManager;
    private TutorialCSVManager _csvManager;
    private QuestConducter[] _questConducters;

    private Dictionary<string, string> _currentDialog;
    private int _dialogStartNumber;
    private int _nextDialogID;
    private int _nextQuestNumber = 0;

    private bool _isSkip = false;
    private bool _isDialogEnd = false;

    private void Awake()
    {
        _tutorialManager = GetComponentInParent<TutorialManager>();
        
        _csvManager = _tutorialManager.CSVManager;
        _dialogStartNumber = _csvManager.GetTutorialStartPoint(_tutorialNumber);

        _questConducters = GetComponentsInChildren<QuestConducter>();
        foreach(QuestConducter quest in _questConducters)
        {
            quest.OnQuestEnd -= QuestEnd;
            quest.OnQuestEnd += QuestEnd;
            quest.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        ResetTutorial();
        ShowNextDialog();
    }

    /// <summary>
    /// Ʃ�丮�� �ʱ�ȭ
    /// </summary>
    private void ResetTutorial()
    {
        StopAllCoroutines();
        foreach (QuestConducter quest in _questConducters)
        {
            quest.gameObject.SetActive(false);
        }

        _isDialogEnd = _isSkip = false;
        _nextDialogID = _dialogStartNumber;
        _nextQuestNumber = 0;
    }

    private void Update()
    {
        _isSkip = OVRInput.GetDown(_skipButton);

        // ��簡 �� ��µ� ��Ȳ
        if(_isDialogEnd && _isSkip)
        {
            _isDialogEnd = _isSkip = false;

            // ������ ����Ʈ���� �Ǻ�
            if(_currentDialog[TutorialField.IsQuest].Length > 0)
            {
                _questConducters[_nextQuestNumber].gameObject.SetActive(true);
                _tutorialManager.ShowQuestText(_currentDialog[TutorialField.IsQuest]);
            }
            // Ʃ�丮���� �������� �Ǵ�
            else if(_nextDialogID == -1)
            {
                _tutorialManager.ShowDialog();
                gameObject.SetActive(false);
            }
            // �� ��(���� ��ȭ�� ���)
            else
            {
                ShowNextDialog();
            }
        }
    }

    private void ShowNextDialog()
    {
        _currentDialog = _csvManager.GetDialog(_nextDialogID);
        _nextDialogID = int.Parse(_currentDialog[TutorialField.Next]);

        StartCoroutine(CoShowDialog());
    }

    private IEnumerator CoShowDialog()
    {
        float elapsedTime = 0f;

        string currentDialogString = _currentDialog[TutorialField.Dialog];
        int dialogLength = currentDialogString.Length;

        string shownDialog = "";
        int currentDialogPosition = 0;

        // �ʱ� ��ȭ ����
        shownDialog += GetNextLetter(ref currentDialogString, ref currentDialogPosition);
        _tutorialManager.ShowDialog(_currentDialog[TutorialField.Name], shownDialog);

        while(currentDialogPosition < dialogLength)
        {
            // ��ŵ�Ǿ�����
            if (_isSkip)
            {
                _tutorialManager.ShowDialog(currentDialogString);
                break;
            }

            elapsedTime += Time.deltaTime;

            // Ư�� �ð��� ������ ���� ���ڸ� ǥ���ؾ� ��
            if(elapsedTime >= _letterPassTime)
            {
                elapsedTime -= _letterPassTime;

                // ���� ���ڰ� ����( )�̶�� �� ���� ���ڸ� �ѹ��� �����
                char nextLetter = GetNextLetter(ref currentDialogString, ref currentDialogPosition);
                if(nextLetter == ' ')
                {
                    shownDialog += nextLetter;
                    nextLetter = GetNextLetter(ref currentDialogString, ref currentDialogPosition);
                }
                shownDialog += nextLetter;
            }
            _tutorialManager.ShowDialog(shownDialog);

            yield return null;
        }

        // ��� ����� ����
        _isDialogEnd = true;
    }

    private char GetNextLetter(ref string dialog, ref int position)
    {
        char nextLetter = dialog[position];
        ++position;
        return nextLetter;
    }

    private void QuestEnd()
    {
        // ����Ʈ�� ������ ��
        ShowNextDialog();
    }
}

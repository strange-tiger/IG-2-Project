//#define _DEV_MODE_

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialNumber = Defines.ELobby2TutorialNumber;
using TutorialField = TutorialCSVManager.TutorialField;

public class TutorialConducter : MonoBehaviour
{
    [SerializeField] private TutorialNumber _tutorialNumber;

    [Header ("Skip")]
    [SerializeField] private OVRInput.Button _skipButton = OVRInput.Button.One;
    [SerializeField] private float _letterPassTime = 0.1f;

    private TutorialManager _tutorialManager;
    private TutorialCSVManager _csvManager;
    private QuestConducter[] _questConducters;

    private Dictionary<string, string> _currentDialogue;
    private int _dialogueStartNumber = -1;
    private int _nextDialogueID;
    private int _nextQuestNumber = 0;

    private bool _isSkip = false;
    private bool _isDialogueEnd = false;

    private void Awake()
    {
        _tutorialManager = GetComponentInParent<TutorialManager>();
        
        _csvManager = _tutorialManager.CSVManager;

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

        _isDialogueEnd = _isSkip = false;
        if (_dialogueStartNumber == -1)
        {
            _dialogueStartNumber = _csvManager.GetTutorialStartPoint(_tutorialNumber);
        }
        _nextDialogueID = _dialogueStartNumber;
        _nextQuestNumber = 0;
    }

    private void Update()
    {
#if _DEV_MODE_
        _isSkip = Input.GetKeyDown(KeyCode.A);
#else
        _isSkip = OVRInput.GetDown(_skipButton);
#endif

        // ��簡 �� ��µ� ��Ȳ
        if(_isDialogueEnd && _isSkip)
        {
            _isDialogueEnd = _isSkip = false;

            // ������ ����Ʈ���� �Ǻ�
            if(_currentDialogue[TutorialField.IsQuest].Length > 0)
            {
                _tutorialManager.ShowQuestText(_currentDialogue[TutorialField.IsQuest]);
                _questConducters[_nextQuestNumber].gameObject.SetActive(true);
            }
            // Ʃ�丮���� �������� �Ǵ�
            else if(_nextDialogueID == -1)
            {
                _tutorialManager.ShowDialogue();
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
        Debug.Log($"[Tutorial] {_tutorialNumber.ToString()} current: {_nextDialogueID}");
        _currentDialogue = _csvManager.GetDialogue(_nextDialogueID);
        _nextDialogueID = int.Parse(_currentDialogue[TutorialField.Next]);
        Debug.Log($"[Tutorial] {_tutorialNumber.ToString()} next:  {_nextDialogueID}");

        StartCoroutine(CoShowDialog());
    }

    private IEnumerator CoShowDialog()
    {
        float elapsedTime = 0f;

        string currentDialogueString = _currentDialogue[TutorialField.Dialogue];
        int dialogueLength = currentDialogueString.Length;

        string shownDialogue = "";
        int currentDialoguePosition = 0;

        // �ʱ� ��ȭ ����
        shownDialogue += GetNextLetter(ref currentDialogueString, ref currentDialoguePosition);
        _tutorialManager.ShowDialogue(_currentDialogue[TutorialField.Name], shownDialogue);

        while(currentDialoguePosition < dialogueLength)
        {
            // ��ŵ�Ǿ�����
            if (_isSkip)
            {
                _tutorialManager.ShowDialogue(currentDialogueString);
                break;
            }

            elapsedTime += Time.deltaTime;

            // Ư�� �ð��� ������ ���� ���ڸ� ǥ���ؾ� ��
            if(elapsedTime >= _letterPassTime)
            {
                elapsedTime -= _letterPassTime;

                // ���� ���ڰ� ����( )�̶�� �� ���� ���ڸ� �ѹ��� �����
                char nextLetter = GetNextLetter(ref currentDialogueString, ref currentDialoguePosition);
                if(nextLetter == ' ')
                {
                    shownDialogue += nextLetter;
                    nextLetter = GetNextLetter(ref currentDialogueString, ref currentDialoguePosition);
                }
                shownDialogue += nextLetter;
            }
            _tutorialManager.ShowDialogue(shownDialogue);

            yield return null;
        }

        // ��� ����� ����
        _isDialogueEnd = true;
    }

    private char GetNextLetter(ref string dialogue, ref int position)
    {
        char nextLetter = dialogue[position];
        ++position;
        return nextLetter;
    }

    private void QuestEnd()
    {
        // ����Ʈ�� ������ ��
        Debug.Log("[Tutorial] Quest ������ ����");
        _tutorialManager.DisableQuestText();
        _questConducters[_nextQuestNumber].gameObject.SetActive(false);
        ++_nextQuestNumber;
        
        if (_nextDialogueID != -1)
        {
            ShowNextDialog();
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }
}

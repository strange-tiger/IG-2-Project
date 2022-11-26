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
    /// 튜토리얼 초기화
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

        // 대사가 다 출력된 상황
        if(_isDialogueEnd && _isSkip)
        {
            _isDialogueEnd = _isSkip = false;

            // 지금이 퀘스트인지 판별
            if(_currentDialogue[TutorialField.IsQuest].Length > 0)
            {
                _tutorialManager.ShowQuestText(_currentDialogue[TutorialField.IsQuest]);
                _questConducters[_nextQuestNumber].gameObject.SetActive(true);
            }
            // 튜토리얼이 끝났는지 판단
            else if(_nextDialogueID == -1)
            {
                _tutorialManager.ShowDialogue();
                gameObject.SetActive(false);
            }
            // 그 외(다음 대화를 출력)
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

        // 초기 대화 세팅
        shownDialogue += GetNextLetter(ref currentDialogueString, ref currentDialoguePosition);
        _tutorialManager.ShowDialogue(_currentDialogue[TutorialField.Name], shownDialogue);

        while(currentDialoguePosition < dialogueLength)
        {
            // 스킵되었는지
            if (_isSkip)
            {
                _tutorialManager.ShowDialogue(currentDialogueString);
                break;
            }

            elapsedTime += Time.deltaTime;

            // 특정 시간이 지나서 다음 글자를 표시해야 함
            if(elapsedTime >= _letterPassTime)
            {
                elapsedTime -= _letterPassTime;

                // 다음 글자가 공백( )이라면 그 다음 글자를 한번에 출력함
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

        // 대사 출력이 끝남
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
        // 퀘스트가 끝났을 때
        Debug.Log("[Tutorial] Quest 끝남을 받음");
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

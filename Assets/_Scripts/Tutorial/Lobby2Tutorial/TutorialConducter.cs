using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TutorialNumber = Defines.ELobby2TutorialNumber;
using TutorialField = TutorialCSVManager.TutorialField;

/// <summary>
/// 대화 진행 등 튜토리얼을 실제로 실행시킴
/// </summary>
public class TutorialConducter : MonoBehaviour
{
    // 튜토리얼 번호
    [SerializeField] private TutorialNumber _tutorialNumber;

    [Header("Sound")]
    [SerializeField] private AudioClip _dialogSound;
    private AudioSource _audioSource;
    
    [Header("Skip")]
    // 대화를 넘길 수 있는 버튼 
    [SerializeField] private OVRInput.Button _skipButton = OVRInput.Button.One;
    [SerializeField] private float _letterPassTime = 0.1f;

    [Header("Quest")]
    // CSV 상에서 퀘스트를 종료를 의미하는 문자열
    [SerializeField] private string _questDisableRequest = "x";
    [SerializeField] private AudioClip _questStartSound;
    [SerializeField] private AudioClip _questEndSound;

    private TutorialManager _tutorialManager;
    private TutorialCSVManager _csvManager;

    private QuestConducter[] _questConducters;
    // 현재까지 활성화되어 있는 퀘스트를 저장해두는 스택
    // 퀘스트 비활성화 요청이 들어오면 해당 퀘스트를 모두 비활성화 해준다.
    private Stack<GameObject> _questStack = new Stack<GameObject>();

    // 현재 대화
    private Dictionary<string, string> _currentDialogue;
    private int _dialogueStartNumber = -1; // 튜토리얼의 시작 대화 번호
    private int _nextDialogueID;
    private int _nextQuestNumber = 0;

    // 현재 대화 상태
    private bool _isSkip = false; // 대화를 넘겼는지
    private bool _isDialogueEnd = false; // 대화 출력이 다 끝났는지

    private void Awake()
    {
        _tutorialManager = GetComponentInParent<TutorialManager>();
        
        _csvManager = _tutorialManager.CSVManager;

        _audioSource = GetComponent<AudioSource>();

        // 자신 하위에 있는 퀘스트 목록을 받아옴(이는 CSV상에 있는 퀘스트 개수와 같아야 한다)
        _questConducters = GetComponentsInChildren<QuestConducter>();
        foreach(QuestConducter quest in _questConducters)
        {
            // 각 퀘스트의 퀘스트 완료 대리자에 함수 연결
            quest.OnQuestEnd -= QuestEnd;
            quest.OnQuestEnd += QuestEnd;
            quest.SetQuestSound(_questStartSound, _questEndSound);
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
        // 퀘스트 초기화
        foreach (QuestConducter quest in _questConducters)
        {
            quest.gameObject.SetActive(false);
        }
        _nextQuestNumber = 0;

        _isDialogueEnd = _isSkip = false;
        // 튜토리얼 시작 시점 저장
        if (_dialogueStartNumber == -1)
        {
            _dialogueStartNumber = _csvManager.GetTutorialStartPoint(_tutorialNumber);
        }
        _nextDialogueID = _dialogueStartNumber;
    }

    private void Update()
    {
        // 스킵 및 대화 넘기기
        _isSkip = OVRInput.GetDown(_skipButton);

        // 대사가 다 출력된 상황
        if(_isDialogueEnd && _isSkip)
        {
            _isDialogueEnd = _isSkip = false;

            string isQuest = _currentDialogue[TutorialField.IsQuest];
            // 지금이 퀘스트인지 판별
            if (isQuest.Length > 0)
            {
                // 퀘스트 종료 요청이면
                if (isQuest == _questDisableRequest)
                {
                    // 현재까지 활성화되어 있는 퀘스트를 종료 시키고 다음 대화를 출력
                    ResetQuestStack();
                    ShowNextDialog();
                }
                // 일반 퀘스트라면
                else
                {
                    // 퀘스트 설정
                    _tutorialManager.ShowQuestText(_currentDialogue[TutorialField.IsQuest]);
                    _questStack.Push(_questConducters[_nextQuestNumber].gameObject); 
                    _questConducters[_nextQuestNumber].gameObject.SetActive(true);
                }
            }
            // 튜토리얼이 끝났는지 판단
            else if (_nextDialogueID == -1)
            {
                _tutorialManager.ShowDialogue();
                gameObject.SetActive(false);
            }
            // 그 외
            else
            {
                // 다음 대화 출력
                ShowNextDialog();
            }
        }
    }

    /// <summary>
    /// 현재까지 활성화 되어있던 퀘스트들을 비활성화 해줌
    /// </summary>
    private void ResetQuestStack()
    {
        int stackCount = _questStack.Count;
        for (int i = 0; i < stackCount; ++i)
        {
            _questStack.Pop().SetActive(false);
        }
    }

    /// <summary>
    /// 다음 대화를 출력
    /// </summary>
    private void ShowNextDialog()
    {
        // 다음 대화를 받음
        _currentDialogue = _csvManager.GetDialogue(_nextDialogueID);
        _nextDialogueID = int.Parse(_currentDialogue[TutorialField.Next]);

        _audioSource.PlayOneShot(_dialogSound);

        _isDialogueEnd = _isSkip = false;
        StartCoroutine(CoShowDialog());
    }

    /// <summary>
    /// 대화를 한 글자씩 출력하기 위한 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoShowDialog()
    {
        float elapsedTime = 0f;

        // 현재 출력해야하는 대사
        string currentDialogueString = _currentDialogue[TutorialField.Dialogue];
        int dialogueLength = currentDialogueString.Length;

        // 현재 보여지는 대사
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
                if(nextLetter == ' ' && currentDialoguePosition < currentDialogueString.Length - 1)
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

    /// <summary>
    /// 다음으로 출력할 문자을 반환
    /// </summary>
    /// <param name="dialogue">원본 대사</param>
    /// <param name="position">현재 대사 출력 포인트</param>
    /// <returns>다음 문자</returns>
    private char GetNextLetter(ref string dialogue, ref int position)
    {
        char nextLetter = dialogue[position];
        ++position;
        return nextLetter;
    }

    /// <summary>
    /// 퀘스트가 끝났을 때 호출함
    /// </summary>
    private void QuestEnd()
    {
        _tutorialManager.DisableQuestText();

        ++_nextQuestNumber;
        
        // 대사가 남아있다면
        if (_nextDialogueID != -1)
        {
            // 다음 대사를 출력
            ShowNextDialog();
        } 
        else
        {
            // 튜토리얼을 종료함
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        ResetQuestStack();
        _tutorialManager.DisableQuestText();
    }
}

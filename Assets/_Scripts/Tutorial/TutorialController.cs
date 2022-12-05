using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using _CSV = Asset.ParseCSV.CSVParser;

public class TutorialController : MonoBehaviour
{
    public enum TutorialType
    {
        Lobby1,
    }

    // 스크립터블 오브젝트
    [SerializeField] Lobby1QuestList _lobby1QuestList;

    // NPC 이름
    [SerializeField] private TextMeshProUGUI _tutorialNPCName;

    // 진행중인 퀘스트 내용
    [SerializeField] private TextMeshProUGUI _questText;

    // NPC 대사
    [SerializeField] private TextMeshProUGUI _tutorialDialogueText;
    [SerializeField] private Lobby1TutorialStartButton _lobby1TutorialStartButton;

    // 튜토리얼 종류 선택 현재는 1종류밖에 없어서 
    [SerializeField] private TutorialType _tutorialType;
    //public int TurtorialTypeNum { get { return (int)_tutorialType; } }

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    // 퀘스트 수락 이벤트
    public UnityEvent<int> QuestAcceptEvent = new UnityEvent<int>();

    // 퀘스트 클리어 이벤트
    public UnityEvent<bool> QuestClearEvent = new UnityEvent<bool>();

    // 대사 종료
    private bool _isDialogueEnd;

    // 다음 대사로 넘어가기 위한 변수
    private bool _isNext;

    // 퀘스트 대사
    private bool _sendMessage;

    // 스킵 버튼
    private bool _dialogueSkip;

    private int _dialogueNum = 0;
    public int DialogueNum { get { return _dialogueNum; } }

    private bool _isTutorialQuest;
    public bool IsTutorialQuest { get { return _isTutorialQuest; } set { _isTutorialQuest = value; } }

    void Start()
    {
        QuestAcceptEvent.RemoveListener(QuestAccept);
        QuestAcceptEvent.AddListener(QuestAccept);

        QuestClearEvent.RemoveListener(QuestClear);
        QuestClearEvent.AddListener(QuestClear);

        if (_tutorialType == TutorialType.Lobby1)
        {
            _tutorialNPCName.text = "이시고르 경";

            _lobby1QuestList = _CSV.ParseCSV("Lobby1TutorialCSV", _lobby1QuestList);

            StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
        }
    }

    private void Update()
    {
        _dialogueSkip = (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.A));
        
        if (_dialogueSkip)
        {
            StopAllCoroutines();

            _tutorialDialogueText.text = null;
            _tutorialDialogueText.text = _lobby1QuestList.Dialogue[_dialogueNum];

            StartCoroutine(Next());
        }

        NextDialogue();
        
        if (_isDialogueEnd == true && _isNext == true && !_isTutorialQuest)
        {
            StopAllCoroutines();
            DialogueNumCount();

            if (_tutorialType == TutorialType.Lobby1)
            {
                if (_dialogueNum == 4 && !_sendMessage)
                {
                    StopAllCoroutines();
                    _tutorialDialogueText.text = null;
                    _tutorialDialogueText.text = _lobby1QuestList.Dialogue[3];
                    _dialogueNum = 3;
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
                    _isDialogueEnd = false;
                    _sendMessage = false;
                }
            }
        }
    }

    /// <summary>
    /// 텍스트 타이핑 효과
    /// </summary>
    /// <param name="dialogue"></param>
    /// <returns></returns>
    IEnumerator TextTyping(string dialogue)
    {
        foreach (char c in dialogue)
        {
            _tutorialDialogueText.text += c;

            yield return _delayTime;
        }

        _isDialogueEnd = true;
    }

    IEnumerator Next()
    {
        yield return _delayTime;

        _isDialogueEnd = true;
    }

    /// <summary>
    /// List의 다음 인덱스 값으로 넘어감
    /// </summary>
    private void DialogueNumCount()
    {
        if (_isDialogueEnd == true && _isNext == true)
        {
            ++_dialogueNum;

            _isDialogueEnd = false;
            _isNext = false;
        }
    }

    /// <summary>
    /// 다음 대화
    /// </summary>
    private void NextDialogue()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && _isDialogueEnd == true)
        {
            _tutorialDialogueText.text = null;
            _isNext = true;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A) && _isDialogueEnd == true)
        {
            _tutorialDialogueText.text = null;
            _isNext = true;
        }
#endif
    }

    /// <summary>
    /// 퀘스트가 있을 때..?
    /// </summary>
    private void QuestClear(bool value)
    {
        if (_isTutorialQuest == true)
        {
            if (_tutorialType == TutorialType.Lobby1)
            {
                _isTutorialQuest = value;
                _lobby1TutorialStartButton.IsQuest = value;
                _sendMessage = value;
            }
        }
    }

    private void QuestAccept(int num)
    {
        if (num == 3)
        {
            _dialogueNum = num;
        }
        else
        {
            _dialogueNum = num;
            _sendMessage = true;
            _isTutorialQuest = false;
        }
    }

    private void OnDisable()
    {
        QuestAcceptEvent.RemoveListener(QuestAccept);
        QuestClearEvent.RemoveListener(QuestClear);
    }
}

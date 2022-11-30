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
        StartRoom,
        Lobby1,
    }

    [SerializeField] StartRoomQuestList _startRoomQuestList;
    [SerializeField] Lobby1QuestList _lobby1QuestList;

    [SerializeField] private TextMeshProUGUI _tutorialNPCName;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private NewPlayerMove _newPlayerMove;
    [SerializeField] private PlayerControllerMove _playerControllerMove;
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;
    [SerializeField] private TextMeshProUGUI _tutorialDialogueText;
    [SerializeField] private Lobby1TutorialStartButton _lobby1TutorialStartButton;

    [SerializeField] private TutorialType _tutorialType;
    public int TurtorialTypeNum { get { return (int)_tutorialType; } }

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    public UnityEvent<int> QuestAcceptEvent = new UnityEvent<int>();
    public UnityEvent<bool> QuestClearEvent = new UnityEvent<bool>();

    private bool _isDialogueEnd;
    private bool _isNext;
    private bool _sendMessage;
    private bool _dialogueSkip;
    private int _dialogueMaxNum;

    private float _curTime;
    private float _requestClearTime = 3f;

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

        if (_tutorialType == TutorialType.StartRoom)
        {
            _tutorialNPCName.text = "요정";

            _startRoomQuestList = _CSV.ParseCSV("StartRoomTutorialCSV", _startRoomQuestList);

            _dialogueMaxNum = _startRoomQuestList.Dialogue.Length;

            StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));
        }

        if (_tutorialType == TutorialType.Lobby1)
        {
            _tutorialNPCName.text = "이시고르 경";

            _lobby1QuestList = _CSV.ParseCSV("Lobby1TutorialCSV", _lobby1QuestList);

            _dialogueMaxNum = _lobby1QuestList.Dialogue.Length;

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

            #region StartRoom
            /* 
            if (_tutorialType == TutorialType.StartRoom)
            {
                StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));

                _isDialogueEnd = false;
            }
            */
            #endregion

            if (_tutorialType == TutorialType.Lobby1)
            {
                if (_dialogueNum == 4 && !_sendMessage)
                {
                    StopAllCoroutines();
                    _tutorialDialogueText.text = null;
                    _tutorialDialogueText.text = _lobby1QuestList.Dialogue[3];
                    _dialogueNum = 3;
                    //_sendMessage = true;
                    Debug.Log("멈춰");
                }
                else
                {
                    StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
                    _isDialogueEnd = false;
                    _sendMessage = false;
                }
            }
        }
        Debug.Log(_dialogueNum);
        Debug.Log($"_sendMessage : {_sendMessage}");
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
        if (OVRInput.GetDown(OVRInput.Button.One) && _isDialogueEnd == true && !_sendMessage)
        {
            _tutorialDialogueText.text = null;
            _isNext = true;
        }
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.A) && _isDialogueEnd == true)
//        {
//            _tutorialDialogueText.text = null;
//            _isNext = true;
//        }
//#endif
    }

    /// <summary>
    /// 퀘스트가 있을 때..?
    /// </summary>
    private void QuestClear(bool value)
    {
        if (_isTutorialQuest == true)
        {
            #region StartRoom
            /*
            if (_tutorialType == TutorialType.StartRoom)
            {
                if (_dialogueNum == 4)
                {
                    _questText.text = "달리기 기능 3초 유지 시 다음으로 넘어감";

                    if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0 || Input.GetKey(KeyCode.F))
                    {
                        _curTime += Time.deltaTime;
                        if (_curTime >= _requestClearTime)
                        {
                            _isTutorialQuest = false;
                            _questText.text = null;
                            _curTime -= _curTime;
                        }
                    }
                    else
                    {
                        _curTime -= _curTime;
                    }
                }


                if (_dialogueNum == 7)
                {
                    _questText.text = "그랩 해 보세요";

                    if (_syncOVRDistanceGrabbable.isGrabbed)
                    {
                        _isTutorialQuest = false;
                        _questText.text = null;
                    }
                }

                if (_dialogueNum == 10)
                {
                    _questText.text = "레이캐스트를 이용 해 그랩 해 보세요";

                    if (_syncOVRDistanceGrabbable.isGrabbed)
                    {
                        _isTutorialQuest = false;
                        _questText.text = null;
                    }
                }
            }
            */
            #endregion

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
        _tutorialDialogueText.text = null;
        _sendMessage = true;
        _dialogueNum = num;
    }

    private void OnDisable()
    {
        QuestAcceptEvent.RemoveListener(QuestAccept);
        QuestClearEvent.RemoveListener(QuestClear);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _CSV = Asset.ParseCSV.CSVParser;

public class StartRoomTutorial : MonoBehaviour
{
    private List<string> _tutorialRunList = new List<string>();
    private List<string> _tutorialGrabberList = new List<string>();
    private List<string> _tutorialRayList = new List<string>();

    [SerializeField] private NewPlayerMove _newPlayerMove;
    [SerializeField] private PlayerControllerMove _playerControllerMove;
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    public enum TurtorialType
    {
        Run,
        Grabber,
        Ray,
        End,
    }

    [SerializeField] private TextMeshProUGUI _tutorialRunText;
    [SerializeField] private TextMeshProUGUI _tutorialNPCName;
    [SerializeField] private TurtorialType _turtorialType;
    public int TurtorialTypeNum { get { return (int)_turtorialType; } }

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    private bool _isDialogueEnd;
    private bool _isNext;
    private int _dialogueMaxNum;
    private bool _dialogueSkip;

    private bool _isRunText = true;
    private bool _isGrabberText = true;
    private bool _isRayText = true;

    private float _curTime;
    private float _requestClearTime = 3f;

    private int _dialogueNum = 0;
    public int DialogueNum { get { return _dialogueNum; } }

    private bool _isTutorialQuest;
    public bool IsTutorialQuest { get { return _isTutorialQuest; } }

    void Start()
    {
        // _newPlayerMove.enabled = false;
        // _playerControllerMove.enabled = false;

        _CSV.ParseCSV("StartRoomTutorialRun", _tutorialRunList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialGrabber", _tutorialGrabberList, '\n', ',');
        _CSV.ParseCSV("StartRoomTutorialRay", _tutorialRayList, '\n', ',');

        if (_turtorialType == TurtorialType.Run && _isRunText)
        {
            _dialogueMaxNum = _tutorialRunList.Count;
            StartCoroutine(TextTyping(_tutorialRunList[_dialogueNum]));
        }
        if (_turtorialType == TurtorialType.Grabber)
        {
            StartCoroutine(TextTyping(_tutorialGrabberList[_dialogueNum]));
        }
        if (_turtorialType == TurtorialType.Ray)
        {
            StartCoroutine(TextTyping(_tutorialRayList[_dialogueNum]));
        }
        _tutorialNPCName.text = "요정";
    }

    private void Update()
    {
        _dialogueSkip = (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.A));

        if (_dialogueSkip)
        {
            StopAllCoroutines();

            _tutorialRunText.text = null;
            _tutorialRunText.text = _tutorialRunList[_dialogueNum];

            StartCoroutine(Next());
        }

        if (_isDialogueEnd == true && _isNext == true && !_isTutorialQuest)
        {
            StopAllCoroutines();
            DialogueNumCount();

            if (_turtorialType == TurtorialType.Run && _isRunText)
            {
                StartCoroutine(TextTyping(_tutorialRunList[_dialogueNum]));
                if (_dialogueNum == 4)
                {
                    _newPlayerMove.enabled = true;
                    _playerControllerMove.enabled = true;

                    _isTutorialQuest = true;
                }
                else
                {
                    _isTutorialQuest = false;
                }
            }

            if (_turtorialType == TurtorialType.Grabber && _isGrabberText)
            {
                _dialogueMaxNum = _tutorialGrabberList.Count;
                StartCoroutine(TextTyping(_tutorialGrabberList[_dialogueNum]));

                if (_dialogueNum == 1 && !_syncOVRDistanceGrabbable.isGrabbed)
                {
                    _isTutorialQuest = true;
                }
            }

            if (_turtorialType == TurtorialType.Ray && _isRayText)
            {
                _dialogueMaxNum = _tutorialRayList.Count;
                StartCoroutine(TextTyping(_tutorialRayList[_dialogueNum]));

                if (_dialogueNum == 1 && !_syncOVRDistanceGrabbable.isGrabbed)
                {
                    _isTutorialQuest = true;
                }
            }

            _isDialogueEnd = false;
        }

        NextDialogue();
        RunQuest();
    }

    /// <summary>
    /// 텍스트 타이핑 효과
    /// </summary>
    /// <param name="dialogue">출력해야 하는 대사 List</param>
    /// <returns></returns>
    IEnumerator TextTyping(string dialogue)
    {
        foreach (char c in dialogue)
        {
            _tutorialRunText.text += c;

            yield return _delayTime;
        }
        
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

            if (_dialogueNum > _dialogueMaxNum - 1)
            {
                ++_turtorialType;

                if (_turtorialType == TurtorialType.Grabber)
                {
                    _isRunText = false;
                    _isGrabberText = true;
                    _isRayText = false;
                }

                if (_turtorialType == TurtorialType.Ray)
                {
                    _isRunText = false;
                    _isGrabberText = false;
                    _isRayText = true;
                }

                if (_turtorialType == TurtorialType.End)
                {
                    gameObject.SetActive(false);
                }

                _isNext = false;

                _dialogueNum -= _dialogueNum;
            }
        }
    }

    /// <summary>
    /// 다음 대화
    /// </summary>
    private void NextDialogue()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && _isDialogueEnd == true)
        {
            _tutorialRunText.text = null;
            _isNext = true;
        }
        else
        {
            _isNext = false;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A) && _isDialogueEnd == true)
        {
            _tutorialRunText.text = null;
            _isNext = true;
        }
        else
        {
            _isNext = false;
        }
#endif
    }

    public virtual void RunQuest()
    {
        if (_isTutorialQuest == true)
        {
            if (_turtorialType == TurtorialType.Run && _isRunText)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0 || Input.GetKey(KeyCode.F))
                {
                    _curTime += Time.deltaTime;
                    if (_curTime >= _requestClearTime)
                    {
                        _isTutorialQuest = false;
                        _curTime -= _curTime;
                    }
                }
                else
                {
                    _curTime -= _curTime;
                }
            }
        }

        if (_turtorialType == TurtorialType.Grabber && _isGrabberText)
        {
            if (_syncOVRDistanceGrabbable.isGrabbed)
            {
                _isTutorialQuest = false;
            }
        }

        if (_turtorialType == TurtorialType.Ray && _isRayText)
        {
            if (_syncOVRDistanceGrabbable.isGrabbed)
            {
                _isTutorialQuest = false;
            }
        }
    }

    IEnumerator Next()
    {
        yield return _delayTime;

        _isDialogueEnd = true;
    }
}

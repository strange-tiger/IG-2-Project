using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _CSV = Asset.ParseCSV.CSVParser;

public class StartRoomTutorial : MonoBehaviour
{
    private List<string> _tutorialCSV = new List<string>();
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

    [SerializeField] private TextMeshProUGUI _questText;
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

        _dialogueMaxNum = _tutorialCSV.Count;

        _CSV.ParseCSV("StartRoomTutorialCSV", _tutorialCSV, '\n', ',');

        if (_turtorialType == TurtorialType.Run && _isRunText)
        {
            StartCoroutine(TextTyping(_tutorialCSV[_dialogueNum]));
        }

        _tutorialNPCName.text = "요정";
    }

    private void Update()
    {
        Debug.Log(_dialogueNum);

        _dialogueSkip = (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.A));

        if (_dialogueSkip)
        {
            StopAllCoroutines();

            _tutorialRunText.text = null;
            _tutorialRunText.text = _tutorialCSV[_dialogueNum];

            StartCoroutine(Next());
        }

        if (_isDialogueEnd == true && _isNext == true && !_isTutorialQuest)
        {
            StopAllCoroutines();
            DialogueNumCount();

            StartCoroutine(TextTyping(_tutorialCSV[_dialogueNum]));

            if (_dialogueNum == 4)
            {
                _questText.text = "달리기 기능 3초 유지 시 다음으로 넘어감";
                _newPlayerMove.enabled = true;
                _playerControllerMove.enabled = true;

                _isTutorialQuest = true;
            }


            else if (_dialogueNum == 7)
            {
                _questText.text = "그랩 해 보세요";
                _isTutorialQuest = true;
            }

            else if (_dialogueNum == 10)
            {
                _questText.text = "레이캐스트를 이용 해 그랩 해 보세요";
                _isTutorialQuest = true;
            }

            else
            {
                _questText.text = null;
                _isTutorialQuest = false;
            }

            _isDialogueEnd = false;
        }

        if (_dialogueNum == 24)
        {
            gameObject.SetActive(false);
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
            _tutorialRunText.text = null;
            _isNext = true;
        }
        else
        {
            _isNext = false;
        }
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.A) && _isDialogueEnd == true)
//        {
//            _tutorialRunText.text = null;
//            _isNext = true;
//        }
//        else
//        {
//            _isNext = false;
//        }
//#endif
    }

    public virtual void RunQuest()
    {
        if (_isTutorialQuest == true)
        {
            if (_isRunText)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0 || Input.GetKey(KeyCode.F))
                {
                    _curTime += Time.deltaTime;
                    if (_curTime >= _requestClearTime)
                    {
                        _questText.text = "Clear";
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

        if ( _isGrabberText)
        {
            if (_syncOVRDistanceGrabbable.isGrabbed || Input.GetKey(KeyCode.Q))
            {
                _questText.text = "Clear";
                _isTutorialQuest = false;
            }
        }

        if (_isRayText)
        {
            if (_syncOVRDistanceGrabbable.isGrabbed || Input.GetKey(KeyCode.W))
            {
                _questText.text = "Clear";
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

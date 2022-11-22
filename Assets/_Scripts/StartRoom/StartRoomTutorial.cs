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

    public enum TurtorialType
    {
        Run,
        Grabber,
        Ray,
        End,
    }

    [SerializeField] private TextMeshProUGUI _tutorialRunText;
    [SerializeField] private TurtorialType _turtorialType;
    public int TurtorialTypeNum { get { return (int)_turtorialType; }}

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    private bool _isDialogueEnd;
    private bool _isNext;
    private int _dialogueMaxNum;

    private int _dialogueNum;
    public int DialogueNum { get { return _dialogueNum; }}

    void Start()
    {
        _newPlayerMove.enabled = false;
        _playerControllerMove.enabled = false;

        if (_turtorialType == TurtorialType.Run)
        {
            _CSV.ParseCSV("StartRoomTutorialRun", _tutorialRunList, '\n', ',');
            _dialogueMaxNum = _tutorialRunList.Count;
            StartCoroutine(TextTyping(_tutorialRunList[_dialogueNum]));
        }

        else if (_turtorialType == TurtorialType.Grabber)
        {
            _CSV.ParseCSV("StartRoomTutorialGrabber", _tutorialGrabberList, '\n', ',');
            _dialogueMaxNum = _tutorialGrabberList.Count;
            StartCoroutine(TextTyping(_tutorialGrabberList[_dialogueNum]));
        }

        else if (_turtorialType == TurtorialType.Ray)
        {
            _CSV.ParseCSV("StartRoomTutorialRay", _tutorialRayList, '\n', ',');
            _dialogueMaxNum = _tutorialRayList.Count;
            StartCoroutine(TextTyping(_tutorialRayList[_dialogueNum]));
        }
    }

    private void Update()
    {
        if (_isDialogueEnd == true && _isNext == true)
        {
            if (_turtorialType == TurtorialType.Run)
            {
                DialogueNumCount();
                StartCoroutine(TextTyping(_tutorialRunList[_dialogueNum]));
                if (_dialogueNum >= 4)
                {
                    _newPlayerMove.enabled = true;
                    _playerControllerMove.enabled = true;
                }
            }

            else if (_turtorialType == TurtorialType.Grabber)
            {
                DialogueNumCount();
                StartCoroutine(TextTyping(_tutorialGrabberList[_dialogueNum]));
            }

            else if (_turtorialType == TurtorialType.Ray)
            {
                DialogueNumCount();
                StartCoroutine(TextTyping(_tutorialRayList[_dialogueNum]));
            }

            _isDialogueEnd = false;
        }
        NextDialogue();
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
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.K))
            {
                _tutorialRunText.text = dialogue;

                StopCoroutine(TextTyping(dialogue));

                _isDialogueEnd = true;

                yield break;
            }
#endif
            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                _tutorialRunText.text = dialogue;

                StopCoroutine(TextTyping(dialogue));

                _isDialogueEnd = true;

                yield break;
            }
        }

        _isDialogueEnd = true;
    }

    /// <summary>
    /// CSV다음 대화를 불러옴
    /// </summary>
    private void DialogueNumCount()
    {
        if (_isDialogueEnd == true && _isNext == true)
        {
            if (_turtorialType == TurtorialType.Run)
            {
                ++_dialogueNum;
                DialogueEnd();
            }

            else if (_turtorialType == TurtorialType.Grabber)
            {
                ++_dialogueNum;
                DialogueEnd();
            }

            else if (_turtorialType == TurtorialType.Ray)
            {
                ++_dialogueNum;
                DialogueEnd();
            }
        }
    }

    /// <summary>
    /// 대화 스킾 기능
    /// </summary>
    private void NextDialogue()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && _isDialogueEnd == true)
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

    /// <summary>
    /// 대화 종료
    /// </summary>
    private void DialogueEnd()
    {
        if (_dialogueNum > _dialogueMaxNum)
        {
            gameObject.SetActive(false);
        }
    }
}

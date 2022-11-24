using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using _CSV = Asset.ParseCSV.CSVParser;

public class TutorialController : MonoBehaviour
{
    [SerializeField] StartRoomQuestList _startRoomQuestList;
    [SerializeField] Lobby1QuestList _lobby1QuestList;

    [SerializeField] private TextMeshProUGUI _tutorialNPCName;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private NewPlayerMove _newPlayerMove;
    [SerializeField] private PlayerControllerMove _playerControllerMove;
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    public enum TutorialType
    {
        StartRoom,
        Lobby1,
    }

    [SerializeField] private TextMeshProUGUI _tutorialDialogueText;
    [SerializeField] private TutorialType _tutorialType;
    public int TurtorialTypeNum { get { return (int)_tutorialType; } }

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    private bool _isDialogueEnd;
    private bool _isNext;
    private int _dialogueMaxNum;

    private float _curTime;
    private float _requestClearTime = 3f;

    private int _dialogueNum = 0;
    public int DialogueNum { get { return _dialogueNum; } }

    private bool _isTutorialQuest;
    public bool IsTutorialQuest { get { return _isTutorialQuest; } }

    void Start()
    {
        if (_tutorialType == TutorialType.StartRoom)
        {
            // _newPlayerMove.enabled = false;
            // _playerControllerMove.enabled = false;

            _tutorialNPCName.text = "요정";

            _startRoomQuestList = _CSV.ParseCSV("StartRoomTutorialCSV", _startRoomQuestList);

            _dialogueMaxNum = _startRoomQuestList.Dialogue.Length;

            StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));
        }

        if (_tutorialType == TutorialType.Lobby1)
        {
            _tutorialNPCName.text = "이시고르 경";

            _lobby1QuestList = _CSV.ParseCSV("Lobby1TutorialCSV", _lobby1QuestList);

            _dialogueNum = _lobby1QuestList.Dialogue.Length;

            StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
        }
    }

    private void Update()
    {
        if (_isDialogueEnd == true && _isNext == true && !_isTutorialQuest)
        {
            DialogueNumCount();

            if (_tutorialType == TutorialType.StartRoom)
            {
                StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));

                _isTutorialQuest = _startRoomQuestList.IsQuest[_dialogueNum];

                _isDialogueEnd = false;
            }

            if (_tutorialType == TutorialType.Lobby1)
            {
                StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));

                _isTutorialQuest = _lobby1QuestList.IsQuest[_dialogueNum];

                StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
            }
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
            _tutorialDialogueText.text += c;

            yield return _delayTime;
            //#if UNITY_EDITOR
            //            if (Input.GetKeyDown(KeyCode.K))
            //            {
            //                _tutorialRunText.text = dialogue;

            //                StopCoroutine(TextTyping(dialogue));

            //                _isDialogueEnd = true;

            //                yield break;
            //            }
            //#endif
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                _tutorialDialogueText.text = dialogue;

                StopCoroutine(TextTyping(dialogue));

                _isDialogueEnd = true;

                yield break;
            }
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
                _tutorialNPCName.text = null;

                gameObject.SetActive(false);

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
            _tutorialDialogueText.text = null;
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

    /// <summary>
    /// 퀘스트가 있을 때..?
    /// </summary>
    public virtual void RunQuest()
    {
        if (_isTutorialQuest == true)
        {
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

            if (_tutorialType == TutorialType.Lobby1)
            {

            }
        }
    }
}

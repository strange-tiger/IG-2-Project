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
            _tutorialNPCName.text = "����";

            _startRoomQuestList = _CSV.ParseCSV("StartRoomTutorialCSV", _startRoomQuestList);

            _dialogueMaxNum = _startRoomQuestList.Dialogue.Length;

            StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));
        }

        if (_tutorialType == TutorialType.Lobby1)
        {
            _tutorialNPCName.text = "�̽ð� ��";

            _lobby1QuestList = _CSV.ParseCSV("Lobby1TutorialCSV", _lobby1QuestList);

            _dialogueMaxNum = _lobby1QuestList.Dialogue.Length;

            StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
        }
    }

    private void Update()
    {
        Debug.Log(_dialogueNum);
        if (_isDialogueEnd == true && _isNext == true && !_isTutorialQuest)
        {
            DialogueNumCount();

            if (_tutorialType == TutorialType.StartRoom)
            {
                StartCoroutine(TextTyping(_startRoomQuestList.Dialogue[_dialogueNum]));

                _isDialogueEnd = false;
            }

            if (_tutorialType == TutorialType.Lobby1)
            {
                if (_dialogueNum == 4 && !_sendMessage)
                {
                    StopCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
                    _dialogueNum = 3;
                }
                else
                {
                    StartCoroutine(TextTyping(_lobby1QuestList.Dialogue[_dialogueNum]));
                    _isDialogueEnd = false;
                    _sendMessage = false;
                }

                _dialogueSkip = OVRInput.GetDown(OVRInput.Button.One);
            }
        }
        NextDialogue();
    }

    /// <summary>
    /// �ؽ�Ʈ Ÿ���� ȿ��
    /// </summary>
    /// <param name="dialogue">����ؾ� �ϴ� ��� List</param>
    /// <returns></returns>
    IEnumerator TextTyping(string dialogue)
    {
        if (_dialogueSkip)
        {
            _tutorialDialogueText.text = dialogue;

            StopCoroutine(TextTyping(dialogue));

            _isDialogueEnd = true;

            yield break;
        }

        foreach (char c in dialogue)
        {
            _tutorialDialogueText.text += c;

            yield return _delayTime;
        }

        _isDialogueEnd = true;
    }

    /// <summary>
    /// List�� ���� �ε��� ������ �Ѿ
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
    /// ���� ��ȭ
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
//            _tutorialDialogueText.text = null;
//            _isNext = true;
//        }
//        else
//        {
//            _isNext = false;
//        }
//#endif
    }

    /// <summary>
    /// ����Ʈ�� ���� ��..?
    /// </summary>
    private void QuestClear(bool value)
    {
        if (_isTutorialQuest == true)
        {
            if (_tutorialType == TutorialType.StartRoom)
            {
                if (_dialogueNum == 4)
                {
                    _questText.text = "�޸��� ��� 3�� ���� �� �������� �Ѿ";

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
                    _questText.text = "�׷� �� ������";

                    if (_syncOVRDistanceGrabbable.isGrabbed)
                    {
                        _isTutorialQuest = false;
                        _questText.text = null;
                    }
                }

                if (_dialogueNum == 10)
                {
                    _questText.text = "����ĳ��Ʈ�� �̿� �� �׷� �� ������";

                    if (_syncOVRDistanceGrabbable.isGrabbed)
                    {
                        _isTutorialQuest = false;
                        _questText.text = null;
                    }
                }
            }

            if (_tutorialType == TutorialType.Lobby1)
            {
                _isTutorialQuest = value;

                _lobby1TutorialStartButton.IsQuest = value;
            }
        }
    }

    private void QuestAccept(int num)
    {
        _dialogueNum = num;

        _sendMessage = true;
    }

    private void OnDisable()
    {
        QuestAcceptEvent.RemoveListener(QuestAccept);
        QuestClearEvent.RemoveListener(QuestClear);
    }
}

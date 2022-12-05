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

    // ��ũ���ͺ� ������Ʈ
    [SerializeField] Lobby1QuestList _lobby1QuestList;

    // NPC �̸�
    [SerializeField] private TextMeshProUGUI _tutorialNPCName;

    // �������� ����Ʈ ����
    [SerializeField] private TextMeshProUGUI _questText;

    // NPC ���
    [SerializeField] private TextMeshProUGUI _tutorialDialogueText;
    [SerializeField] private Lobby1TutorialStartButton _lobby1TutorialStartButton;

    // Ʃ�丮�� ���� ���� ����� 1�����ۿ� ��� 
    [SerializeField] private TutorialType _tutorialType;
    //public int TurtorialTypeNum { get { return (int)_tutorialType; } }

    private WaitForSeconds _delayTime = new WaitForSeconds(0.1f);

    // ����Ʈ ���� �̺�Ʈ
    public UnityEvent<int> QuestAcceptEvent = new UnityEvent<int>();

    // ����Ʈ Ŭ���� �̺�Ʈ
    public UnityEvent<bool> QuestClearEvent = new UnityEvent<bool>();

    // ��� ����
    private bool _isDialogueEnd;

    // ���� ���� �Ѿ�� ���� ����
    private bool _isNext;

    // ����Ʈ ���
    private bool _sendMessage;

    // ��ŵ ��ư
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
            _tutorialNPCName.text = "�̽ð� ��";

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
    /// �ؽ�Ʈ Ÿ���� ȿ��
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
    /// List�� ���� �ε��� ������ �Ѿ
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
    /// ���� ��ȭ
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
    /// ����Ʈ�� ���� ��..?
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

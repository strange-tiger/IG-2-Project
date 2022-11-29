using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

using _CSV = Asset.ParseCSV.CSVParser;

public class InputTutorial : MonoBehaviour
{
    [Header("Tutorial Conversation")]
    [SerializeField] private GameObject _conversationUI;
    [SerializeField] private TextMeshProUGUI _conversationText;
    [SerializeField] private AudioClip _dialogueSound;

    [Header("Trigger")]
    [SerializeField] private InputTutorialTrigger _trigger;
    [SerializeField] private GameObject _triggerObject;
    [SerializeField] private FocusableObjects _buttonTriggerObject;

    [Header("MakeCharacter UI")]
    [SerializeField] private MakeCharacterManager _makeCharacterManager;
    [SerializeField] private Button _characterMakeButton;
    [SerializeField] private Button _femaleButton;

    private List<string> _conversationList = new List<string>();
    private AudioSource _audioSource;
    private Coroutine ConversationCoroutine;

    private int _indexNum = 0;
    private int[] _pauseNum = { 4, 8, 11, 15, 18 };
    private int _pauseIndexNum;
    private bool _isPause;

    private void OnEnable()
    {
        _trigger.OnTriggered.RemoveListener(ConversationRestart);
        _trigger.OnTriggered.AddListener(ConversationRestart);
        _makeCharacterManager.OnClickFemaleButton.RemoveListener(ConversationRestart);
        _makeCharacterManager.OnClickFemaleButton.AddListener(ConversationRestart);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        _conversationList = _CSV.ParseCSV("InputTutorial", _conversationList);
        ConversationCoroutine = StartCoroutine(CoConversationPrint());
    }

    private void Update()
    {
        if (_conversationUI.activeSelf)
        {
            PlayerControlManager.Instance.IsMoveable = false;
        }
        else
        {
            PlayerControlManager.Instance.IsMoveable = true;
        }

        if (_indexNum == _pauseNum[_pauseIndexNum])
        {
            ConversationPause();
        }

        if (_isPause == false)
        {
            ConversationSkip();
        }
    }

    private IEnumerator CoConversationPrint()
    {
        for (int i = 0; i < _conversationList[_indexNum].Length; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            _conversationText.text += _conversationList[_indexNum][i];
        }
    }

    private void ConversationSkip()
    {
        if (_conversationText.text != null)
        {
            if (_conversationText.text.Length != _conversationList[_indexNum].Length)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    StopCoroutine(ConversationCoroutine);
                    _conversationText.text = _conversationList[_indexNum];
                }
            }
            else
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    ++_indexNum;
                    _audioSource.PlayOneShot(_dialogueSound);
                    _conversationText.text = null;
                    ConversationCoroutine = StartCoroutine(CoConversationPrint());
                }
            }
        }
    }

    private void ConversationPause()
    {
        _isPause = true;

        _conversationUI.SetActive(false);

        switch (_pauseIndexNum)
        {
            case 0:
                _triggerObject.gameObject.SetActive(true);
                _trigger.enabled = true;
                _pauseIndexNum++;
                return;
            case 1:
                StartCoroutine(CoRotatePlayerTutorialDelay());
                return;
            case 2:
                _triggerObject.gameObject.SetActive(true);
                _trigger.enabled = true;
                _pauseIndexNum++;
                return;
            case 3:
                _buttonTriggerObject.enabled = true;
                _buttonTriggerObject.OnFocus();
                _femaleButton.interactable = true;
                _pauseIndexNum++;
                return;
            case 4:
                _characterMakeButton.interactable = true;
                _makeCharacterManager.OnClickFemaleButton.RemoveListener(ConversationRestart);
                return;
        }
    }

    private IEnumerator CoRotatePlayerTutorialDelay()
    {
        yield return new WaitForSeconds(5f);

        _indexNum++;
    }

    private void ConversationRestart()
    {
        _isPause = false;
        _conversationUI.SetActive(true);
    }

    private void OnDisable()
    {
        _trigger.OnTriggered.RemoveListener(ConversationRestart);
    }
}

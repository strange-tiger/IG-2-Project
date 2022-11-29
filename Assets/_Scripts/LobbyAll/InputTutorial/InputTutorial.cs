using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

using _CSV = Asset.ParseCSV.CSVParser;

public class InputTutorial : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _conversationText;
    [SerializeField] GameObject _conversationUI;
    [SerializeField] InputTutorialTrigger _trigger;
    [SerializeField] GameObject _triggerObject;
    [SerializeField] FocusableObjects _buttonTriggerObject;
    [SerializeField] MakeCharacterManager _makeCharacterManager;
    [SerializeField] Button _characterMakeButton;
    [SerializeField] Button _femaleButton;
    [SerializeField] AudioClip _dialogueSound;

    private AudioSource _audioSource;
    private List<string> _conversationList = new List<string>();
    private Coroutine ConversationCoroutine;
    private bool _isPause;
    private int[] _pauseNum = { 4,8,11,15,18 };
    private int _indexNum = 0;
    private int _pauseIndexNum;

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
       
        if(_conversationUI.activeSelf)
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
        if(_conversationText.text != null)
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

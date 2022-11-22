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
    [SerializeField] Button _characterMakeButton;
    [SerializeField] Button _femaleButton;

    private List<string> _conversationList = new List<string>();
    private int _indexNum = -1;
    private bool _conversationEnd = true;
    private Coroutine ConversationCoroutine;
    private bool _isPause;
    private int[] _pauseNum = { 3,6,11,13 };
    private int _pauseIndexNum;

    private void OnEnable()
    {
        _trigger.OnTriggered.RemoveListener(ConversationRestart);
        _trigger.OnTriggered.AddListener(ConversationRestart);
    }

    void Start()
    {
        _conversationList = _CSV.ParseCSV("InputTutorial", _conversationList);

    }

    private void Update()
    {
        if(_indexNum == _pauseNum[_pauseIndexNum])
        {
            ConversationPause();
        }

        if (_isPause == false)
        {
            ConversationSkip();
        }

    }
    private IEnumerator ConversationPrint()
    {
        for(int i = 0; i < _conversationList[_indexNum].Length; ++i)
        {
            yield return new WaitForSeconds(0.1f);

            _conversationText.text += _conversationList[_indexNum][i];
        }
        _conversationEnd = true;

    }

    private void ConversationSkip()
    {
        if (_conversationEnd == false)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                StopCoroutine(ConversationCoroutine);
                _conversationText.text = _conversationList[_indexNum];
                _conversationEnd = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                _conversationText.text = null;
                ++_indexNum;
                _conversationEnd = false;

                ConversationCoroutine = StartCoroutine(ConversationPrint());

            }
        }
    }

    private void ConversationPause()
    {
        _isPause = true;
        _conversationUI.SetActive(false);
        _pauseIndexNum++;

        switch (_pauseIndexNum)
        {
            case 1:
                _trigger.gameObject.SetActive(true);
                return;
            case 2:
                _femaleButton.GetComponent<FocusableObjects>().OnFocus();
                return;
            case 3:
                _characterMakeButton.interactable = true;
                return;
        }

    }

    private void ConversationRestart()
    {
        _isPause = false;
        _conversationUI.SetActive(true);
        _indexNum++;
        _conversationEnd = false;
    }

    private void OnDisable()
    {
        _trigger.OnTriggered.RemoveListener(ConversationRestart);
    }
}

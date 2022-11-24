using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

using SceneType = Defines.ESceneNumder;
using _CSV = Asset.ParseCSV.CSVParser;


public class ArenaTutorial : MonoBehaviour
{
    [Header("Tutorial Text")]
    [SerializeField] TextMeshProUGUI _conversationText;
    [SerializeField] GameObject _conversationUI;
    [SerializeField] AudioClip _dialogueSound;

    [Header("Restart Trigger")]
    [SerializeField] TutorialBettingUI _tutorialBettingUI;

    private AudioSource _audioSource;
    private List<string> _conversationList = new List<string>();
    private Coroutine ConversationCoroutine;
    private bool _isPause;
    private int _pauseNum = 17;
    private int _indexNum = 0;
    private bool _isFirstVisit;

    private void OnEnable()
    {
        _tutorialBettingUI.OnTriggered.RemoveListener(ConversationRestart);
        _tutorialBettingUI.OnTriggered.AddListener(ConversationRestart);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_isFirstVisit)
        {
            _conversationList = _CSV.ParseCSV("FirstVisitArenaTutorial", _conversationList);
        }
        else
        {
            _conversationList = _CSV.ParseCSV("NotFirstVisitArenaTutorial", _conversationList);
        }


        ConversationCoroutine = StartCoroutine(ConversationPrint());

    }

    private void Update()
    {

        if (_conversationUI.activeSelf)
        {
            PlayerControlManager.Instance.IsMoveable = false;
            PlayerControlManager.Instance.IsRayable = false;
        }
        else
        {
            PlayerControlManager.Instance.IsMoveable = true;
            PlayerControlManager.Instance.IsRayable = true;

        }

        if (_indexNum == _pauseNum)
        {
            ConversationPause();
        }

        if (_isPause == false)
        {
            ConversationSkip();
        }

        if (_indexNum > _conversationList.Count)
        {
            TutorialEnd();
        }
    }
    private IEnumerator ConversationPrint()
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

                    if (_indexNum <= _conversationList.Count)
                    {
                        _audioSource.PlayOneShot(_dialogueSound);
                        _conversationText.text = null;
                        ConversationCoroutine = StartCoroutine(ConversationPrint());
                    }
                }
            }
        }
    }

    private void ConversationPause()
    {
        _isPause = true;
        _conversationUI.SetActive(false);
    }

    private void ConversationRestart()
    {
        _tutorialBettingUI.BettingPanelOff();
        _isPause = false;
        _conversationUI.SetActive(true);
    }

    private void TutorialEnd() => SceneManager.LoadScene((int)SceneType.ArenaRoom);

    private void OnDisable()
    {
        _tutorialBettingUI.OnTriggered.RemoveListener(ConversationRestart);
    }
}

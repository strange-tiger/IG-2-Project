using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

using SceneType = Defines.ESceneNumder;
using _CSV = Asset.ParseCSV.CSVParser;
using Asset.MySql;

public class ArenaTutorial : MonoBehaviourPun
{
    [Header("Tutorial Text")]
    [SerializeField] private TextMeshProUGUI _conversationText;
    [SerializeField] private GameObject _conversationUI;
    [SerializeField] private AudioClip _dialogueSound;

    [Header("Restart Trigger")]
    [SerializeField] private TutorialBettingUI _tutorialBettingUI;

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

        _isFirstVisit = MySqlSetting.CheckCompleteTutorial(PhotonNetwork.NickName, ETutorialCompleteState.ARENA);
        
        _tutorialBettingUI.BettingPanelOff();

        if (_isFirstVisit)
        {
            _conversationList = _CSV.ParseCSV("NotFirstVisitArenaTutorial", _conversationList);
        }
        else
        {
            _conversationList = _CSV.ParseCSV("FirstVisitArenaTutorial", _conversationList);
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

        if (_isPause == false && _indexNum < _pauseNum + 1)
        {
            ConversationSkip();
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
        if (_conversationText.text != null && _conversationUI.activeSelf)
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
                    if (_indexNum == _pauseNum)
                    {
                        ConversationPause();
                    }

                    if (_indexNum == _conversationList.Count - 1)
                    {
                        TutorialEnd();
                    }

                    ++_indexNum;


                    _audioSource.PlayOneShot(_dialogueSound);
                    _conversationText.text = null;
                    ConversationCoroutine = StartCoroutine(ConversationPrint());

                }
            }


        }


    }

    private void ConversationPause()
    {
        _tutorialBettingUI.transform.GetChild(0).gameObject.SetActive(true);

        _isPause = true;
        _conversationUI.SetActive(false);
    }

    private void ConversationRestart()
    {
        _pauseNum = 19;
        _tutorialBettingUI.transform.GetChild(0).gameObject.SetActive(false);
        _conversationUI.SetActive(true);
        _isPause = false;
    }

    private void TutorialEnd()
    {
        if(!_isFirstVisit)
        {
            MySqlSetting.CompleteTutorial(PhotonNetwork.NickName, ETutorialCompleteState.ARENA);
        }

        SceneManager.LoadScene((int)SceneType.ArenaRoom);
    }
    private void OnDisable()
    {
        _tutorialBettingUI.OnTriggered.RemoveListener(ConversationRestart);
    }
}

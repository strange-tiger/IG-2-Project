 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

using SceneType = Defines.ESceneNumber;
using _CSV = Asset.ParseCSV.CSVParser;
using Asset.MySql;

/* 
 * 투기장 튜토리얼을 담당하는 스크립트
 */
public class ArenaTutorial : MonoBehaviourPun
{
    // 튜토리얼 대화에 관련된 텍스트, 사운드, UI.
    [Header("Tutorial Text")]
    [SerializeField] private TextMeshProUGUI _conversationText;
    [SerializeField] private GameObject _conversationUI;
    [SerializeField] private AudioClip _dialogueSound;

    // 튜토리얼 BettingUI를 받아 플레이어가 베팅 체험을 끝내면 다시 대화가 시작되게함.
    [Header("Restart Trigger")]
    [SerializeField] private TutorialBettingUI _tutorialBettingUI;

    // CSV로 파싱 받은 문자열을 저장하는 리스트.
    private List<string> _conversationList = new List<string>();

    private AudioSource _audioSource;

    // 튜토리얼이 끝나고 투기장으로 넘어가기 위한 LobbyChanger.
    private LobbyChange _lobbyChange;

    // 대화를 스킵하기 위해 대화 출력 코루틴을 변수로 저장함.
    private Coroutine ConversationCoroutine;

    // 대화에 사용될 인덱스와 멈춰야하는 인덱스를 저장.
    private int _pauseNum = 17;
    private int _indexNum = 0;

    // 처음 방문과 아닌 경우 대화가 달라지기 때문에 여부를 확인해야함.
    private bool _isNotFirstVisit;
    private bool _isPause;

    private void OnEnable()
    {
        _tutorialBettingUI.OnTriggered.RemoveListener(ConversationRestart);
        _tutorialBettingUI.OnTriggered.AddListener(ConversationRestart);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();


        _lobbyChange = GetComponent<LobbyChange>();

        _tutorialBettingUI.BettingPanelOff();

        // 투기장 첫 방문인지 아닌지를 DB에서 받아와 저장함.
        _isNotFirstVisit = MySqlSetting.CheckCompleteTutorial(PhotonNetwork.NickName, ETutorialCompleteState.ARENA);

        // 방문 여부에 따라 다른 대화 리스트를 적용시킴.
        if (_isNotFirstVisit)
        {
            _conversationList = _CSV.ParseCSV("NotFirstVisitArenaTutorial", _conversationList);
        }
        else
        {
            _conversationList = _CSV.ParseCSV("FirstVisitArenaTutorial", _conversationList);
        }

        // 적용 시킨 대화 리스트로 대화 출력 코루틴을 실행시킴.
        ConversationCoroutine = StartCoroutine(CoConversationPrint());
    }

    private void Update()
    {
        // 튜토리얼 대화 창이 켜져있는 동안은 움직이거나, Ray를 쏠 수 없음.
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

        // 대화가 일시정지 된 상황이아니라면 다음 대화로 넘어가거나 빨리 출력시킬 수 있음.
        if (_isPause == false && _indexNum < _pauseNum + 1)
        {
            ConversationSkip();
        }
    }

    /// <summary>
    /// 한 글자 씩 대화를 출력시키는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoConversationPrint()
    {
        for (int i = 0; i < _conversationList[_indexNum].Length; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            _conversationText.text += _conversationList[_indexNum][i];
        }
    }

    /// <summary>
    /// 플레이어의 입력을 받아 다음 대화로 넘어가거나 출력중인 글자를 빠르게 출력시키는 메서드.
    /// </summary>
    private void ConversationSkip()
    {
        // UI가 켜져있고, 대화창의 Text가 Null이 아닐 때 동작.
        if (_conversationText.text != null && _conversationUI.activeSelf)
        {
            // 아직 출력 중이라면
            if (_conversationText.text.Length != _conversationList[_indexNum].Length)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    // 플레이어의 입력을 받으면 코루틴을 멈추고 바로 대화를 출력함.
                    StopCoroutine(ConversationCoroutine);
                    _conversationText.text = _conversationList[_indexNum];

                }
            }
            else
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    // 아니라면 인덱스를 확인하여 대화를 일시정지 시키거나, 다음 대화로 넘어가거나, 튜토리얼을 종료한다.
                    if (_indexNum == _pauseNum)
                    {
                        ConversationPause();
                    }

                    if (_indexNum == _conversationList.Count - 1)
                    {
                        TutorialEnd();
                    }
                    else
                    {
                        ++_indexNum;
                    }

                    // 넘긴 후에는 정해진 사운드를 출력시키고, 대화창을 Clear하고 다음 인덱스를 받아 다시 코루틴을 실행시킨다.
                    _audioSource.PlayOneShot(_dialogueSound);
                    _conversationText.text = null;
                    ConversationCoroutine = StartCoroutine(CoConversationPrint());

                }
            }
        }
    }

    /// <summary>
    /// 베팅 체험을 위해서 대화를 일시정지 시킴.
    /// </summary>
    private void ConversationPause()
    {
        _tutorialBettingUI.transform.GetChild(0).gameObject.SetActive(true);

        _isPause = true;
        _conversationUI.SetActive(false);
    }

    /// <summary>
    /// 베팅을 완료하면 다시 대화를 재개함.
    /// </summary>
    private void ConversationRestart()
    {
        _pauseNum = 18;
        _tutorialBettingUI.transform.GetChild(0).gameObject.SetActive(false);
        _conversationUI.SetActive(true);
        _isPause = false;
    }

    /// <summary>
    /// 대화 리스트의 마지막 인덱스라면 튜토리얼을 끝냄.
    /// </summary>
    private void TutorialEnd()
    {
        // 첫 방문이라면 투기장 튜토리얼 완료를 DB에 저장함.
        if (_isNotFirstVisit == false)
        {
            MySqlSetting.CompleteTutorial(PhotonNetwork.NickName, ETutorialCompleteState.ARENA);
        }
        _tutorialBettingUI.gameObject.SetActive(false);

        // LobbyChanger와의 상호작용을 호출하여 투기장으로 넘어갈 수 있게함.
        _lobbyChange.Interact();
    }

    private void OnDisable()
    {
        _tutorialBettingUI.OnTriggered.RemoveListener(ConversationRestart);
    }
}

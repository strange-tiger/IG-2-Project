using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

using _CSV = Asset.ParseCSV.CSVParser;
/*
 * 캐릭터 생성씬에서 진행되는 기본적인 플레이어 조작 튜토리얼
 */
public class InputTutorial : MonoBehaviour
{
    // 튜토리얼의 대화와 관련된 사운드와 UI
    [Header("Tutorial Conversation")]
    [SerializeField] private GameObject _conversationUI;
    [SerializeField] private TextMeshProUGUI _conversationText;
    [SerializeField] private AudioClip _dialogueSound;

    // 플레이어를 특정 위치로 이동하면 작동하는 트리거 발판
    [Header("Trigger")]
    [SerializeField] private InputTutorialTrigger _trigger;
    [SerializeField] private GameObject _triggerObject;
    [SerializeField] private FocusableObjects _buttonTriggerObject;

    // 캐릭터를 만드는 UI
    [Header("MakeCharacter UI")]
    [SerializeField] private MakeCharacterManager _makeCharacterManager;
    [SerializeField] private Button _characterMakeButton;
    [SerializeField] private Button _femaleButton;

    // CSV로 파싱 받은 대화 내용 리스트
    private List<string> _conversationList = new List<string>();
    private AudioSource _audioSource;

    // 한 글자 씩 대화를 출력하기 위한 코루틴
    private Coroutine ConversationCoroutine;

    // 대화 리스트의 인덱스와 멈춰야하는 인덱스의 배열, 그리고 그 배열의 인덱스
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

        // CSV를 파싱받아 List에 추가하고, 이를 이용하여 글자를 출력하는 코루틴을 실행 시킴.
        _conversationList = _CSV.ParseCSV("InputTutorial", _conversationList);
        ConversationCoroutine = StartCoroutine(CoConversationPrint());
    }

    private void Update()
    {
        // 대화 UI가 활성화되어 있으면 플레이어는 움직일 수 없음.
        if (_conversationUI.activeSelf)
        {
            PlayerControlManager.Instance.IsMoveable = false;
        }
        else
        {
            PlayerControlManager.Instance.IsMoveable = true;
        }

        // 대화 리스트의 인덱스가 멈춰야하는 인덱스 배열과 일치하면 대화를 일시정지함
        if (_indexNum == _pauseNum[_pauseIndexNum])
        {
            ConversationPause();
        }

        // 대화가 멈춰있지 않아야만 다음 대화로 넘어가거나 빠르게 출력시킬 수 있음
        if (_isPause == false)
        {
            ConversationSkip();
        }
    }

    //  한 글자 씩 대화를 출력시킴
    private IEnumerator CoConversationPrint()
    {
        for (int i = 0; i < _conversationList[_indexNum].Length; ++i)
        {
            yield return new WaitForSeconds(0.1f);
            _conversationText.text += _conversationList[_indexNum][i];
        }
    }

    // 다음 대화로 넘어가거나 대화가 출력 중이라면 빠르게 출력시킴
    private void ConversationSkip()
    {
        if (_conversationText.text != null)
        {
            if (_conversationText.text.Length != _conversationList[_indexNum].Length)
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    // 대화 출력 중에 플레이어의 입력을 받으면 코루틴을 멈추고 대화를 빠르게 출력시킴
                    StopCoroutine(ConversationCoroutine);
                    _conversationText.text = _conversationList[_indexNum];
                }
            }
            else
            {
                if (OVRInput.GetDown(OVRInput.RawButton.A))
                {
                    // 대화가 모두 출력되었다면 사운드를 출력하고 다음 대화로 넘어가는 코루틴을 시작함
                    ++_indexNum;
                    _audioSource.PlayOneShot(_dialogueSound);
                    _conversationText.text = null;
                    ConversationCoroutine = StartCoroutine(CoConversationPrint());
                }
            }
        }
    }

    // 멈춰야하는 인덱스 마다 정해진 동작을 실행함
    private void ConversationPause()
    {
        // 다음 대화로 넘어가지 못하게 만들고
        _isPause = true;

        // 대화 창 UI를 비활성화시킴
        _conversationUI.SetActive(false);

        switch (_pauseIndexNum)
        {
            // 거울 앞 발판을 활성화 하고 여기에 들어가면 다시 시작할 수 있도록 함
            case 0:
                _triggerObject.gameObject.SetActive(true);
                _trigger.enabled = true;
                _pauseIndexNum++;
                return;
            
            // 5초뒤에 다시 시작함
            case 1:
                StartCoroutine(CoRotatePlayerTutorialDelay());
                _pauseIndexNum++;
                return;
                
            // 캐릭터 생성 UI 앞의 발판을 활성화 하고 여기에 들어가면 다시 시작함
            case 2:
                _triggerObject.gameObject.SetActive(true);
                _trigger.enabled = true;
                _pauseIndexNum++;
                return;

            // 여성 성별 버튼에 아웃라인을 출력하고 버튼을 활성화 시킴
            case 3:
                _buttonTriggerObject.enabled = true;
                _buttonTriggerObject.OnFocus();
                _femaleButton.interactable = true;
                _pauseIndexNum++;
                return;

            // 캐릭터 생성이 가능하게끔 하며, 여성 버튼을 눌렀을 때의 이벤트를 비활성화함
            case 4:
                _characterMakeButton.interactable = true;
                _makeCharacterManager.OnClickFemaleButton.RemoveListener(ConversationRestart);
                return;
        }
    }

    // 5초를 대기하는 코루틴
    private IEnumerator CoRotatePlayerTutorialDelay()
    {
        yield return new WaitForSeconds(5f);


        ConversationRestart();
    }

    // 일시정지된 대화를 다시 재개함.
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

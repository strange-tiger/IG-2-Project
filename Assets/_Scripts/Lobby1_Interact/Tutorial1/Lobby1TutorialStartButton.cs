using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class Lobby1TutorialStartButton : MonoBehaviour
{
    // 튜토리얼 버튼들
    [SerializeField] private Button[] _tutorialButton;

    // 튜토리얼 오브젝트들
    [SerializeField] private GameObject[] _tutorialObject;
    [SerializeField] private TutorialController _tutorialController;

    // 진행 중 퀘스트의 텍스트
    [SerializeField] private TextMeshProUGUI _questText;
    // 0 / 0 퀘스트 진행도의 텍스트
    [SerializeField] private TextMeshProUGUI _questProgress;

    [SerializeField] private LobbyChanger _lobbyChanger;
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;

    // 버튼 활성화 이벤트
    private Action OnButtonAction;

    // 현재 퀘스트중이면 true 아니면 false
    private bool _isQuest;
    public bool IsQuest { get { return _isQuest; } set { _isQuest = value; } }

    private bool _isButton;

    // 하드코딩 입니다. 각각 버튼을 눌렀을 때  true 로 바뀌며 누른 버튼의 퀘스트가 완료되면 false 가 됩니다.
    private bool _isOne;
    private bool _isTwo;
    private bool _isThree;
    private bool _isFour;
    private bool _isFive;
    private bool _isSix;

    private void Start()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = false;
        }

        // 각각의 버튼 이벤트
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            int num = i;
            _tutorialButton[i].onClick.RemoveAllListeners();
            _tutorialButton[i].onClick.AddListener(() =>
            {
                OnClickButton(num);
            });
        }

        // 튜토리얼에서 나가는 버튼 이벤트
        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
        _tutorialButton[6].onClick.AddListener(ClickExitButton);

        // 버튼 활성화 시키는 이벤트 등록
        OnButtonAction = OnButtons;
    }

    private void Update()
    {
        if (_tutorialController.DialogueNum == 3 && !_isButton)
        {
            OnButtonAction?.Invoke();
            _isButton = true;
        }

        if (_isOne)
        {
            _questText.text = "발판 7개를 모두 밟아보세요";
            if (_tutorialController.DialogueNum == 5 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 6)
            {
                _isOne = false;
                QuestReset();
            }
        }

        else if (_isTwo)
        {
            _questText.text = "공을 그랩으로 집어서 골대에 넣어보세요";
            if (_tutorialController.DialogueNum == 8 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 10)
            {
                _isTwo = false;
                QuestReset();
            }
        }

        else if (_isThree)
        {
            _questText.text = "마법봉을 주워서 마법을 사용하세요";
            if (_tutorialController.DialogueNum == 16 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 17)
            {
                _isThree = false;
                QuestReset();
            }
        }

        else if (_isFour)
        {
            _questText.text = "음식을 먹고 포만감을 최대 수치까지 채워주세요";
            if (_tutorialController.DialogueNum == 23 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 24)
            {
                _isFour = false;
                QuestReset();
            }
        }

        else if (_isFive)
        {
            _questText.text = "채광에 성공하여 골드를 획득하세요";
            if (_tutorialController.DialogueNum == 35 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 36)
            {
                _isFive = false;
                QuestReset();
            }
        }

        else if (_isSix)
        {
            _questText.text = "무기를 집어 이시고르에게 돌아가세요";
            if (_tutorialController.DialogueNum == 45 && !_isQuest)
            {
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 47)
            {
                _questText.text = "물건을 4개 베세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 54)
            {
                _isSix = false;
                QuestReset();
            }
        }

        #region EDITOR
//#if UNITY_EDITOR
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            OnClickButton(0);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            OnClickButton(1);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha3))
//        {
//            OnClickButton(2);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha4))
//        {
//            OnClickButton(3);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha5))
//        {
//            OnClickButton(4);
//        }
//        if (Input.GetKeyDown(KeyCode.Alpha6))
//        {
//            OnClickButton(5);
//        }
//#endif
        #endregion
    }

    private void OnClickButton(int num)
    {
        _audioSource.PlayOneShot(_audioClips[0]);

        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialObject[i].SetActive(false);
            }
        }

        _tutorialController.IsTutorialQuest = false;
        _isQuest = false;
        _tutorialObject[num].SetActive(true);

        switch (num)
        {
            case 0:
                _tutorialController.QuestAcceptEvent.Invoke(4);
                _isOne = true;
                break;
            case 1:
                _tutorialController.QuestAcceptEvent.Invoke(7);
                _isTwo = true;
                break;
            case 2:
                _tutorialController.QuestAcceptEvent.Invoke(11);
                _isThree = true;
                break;
            case 3:
                _tutorialController.QuestAcceptEvent.Invoke(18);
                _isFour = true;
                break;
            case 4:
                _tutorialController.QuestAcceptEvent.Invoke(25);
                _isFive = true;
                break;
            case 5:
                _tutorialController.QuestAcceptEvent.Invoke(37);
                _isSix = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 버튼 활성화
    /// </summary>
    private void OnButtons()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = true;
        }
    }

    /// <summary>
    /// _tutorialButton[6].onClick.AddListener(ClickExitButton); 에 해당되는 이벤트
    /// </summary>
    private void ClickExitButton()
    {
        _lobbyChanger.ChangeLobby(Defines.ESceneNumber.FantasyLobby);
    }

    /// <summary>
    /// 퀘스트 클리어 혹은 다른 퀘스트 시작 시 호출
    /// </summary>
    private void QuestReset()
    {
        _audioSource.PlayOneShot(_audioClips[1]);
        _tutorialController.QuestAcceptEvent.Invoke(3);
        
        _isQuest = false;
        _isButton = false;
        _questText.text = null;


        if (_questText.text != null)
        {
            _questText.text = null;
        }
        if (_questProgress.text != null)
        {
            _questProgress.text = null;
        }
    }

    private void OnDisable()
    {
        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
    }
}

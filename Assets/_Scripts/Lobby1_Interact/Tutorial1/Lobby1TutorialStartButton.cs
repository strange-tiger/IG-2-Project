using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

public class Lobby1TutorialStartButton : MonoBehaviour
{
    [SerializeField] private Button[] _tutorialButton;
    [SerializeField] private GameObject[] _tutorialObject;
    [SerializeField] private TutorialController _tutorialController;
    [SerializeField] private TextMeshProUGUI _questText;
    [SerializeField] private TextMeshProUGUI _questProgress;
    [SerializeField] private LobbyChanger _lobbyChanger;
    //[SerializeField] private GameObject _image;

    private Action OnButtonAction;

    private bool _firstClick;
    private bool _isQuest;
    public bool IsQuest { get { return _isQuest; } set { _isQuest = value; } }

    private bool _isButton;
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

        for (int i = 0; i < _tutorialButton.Length; ++i)
        {
            int num = i;
            _tutorialButton[i].onClick.RemoveAllListeners();
            _tutorialButton[i].onClick.AddListener(() =>
            {
                OnClickButton(num);
            });
        }

        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
        _tutorialButton[6].onClick.AddListener(ClickExitButton);

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

        if (_isTwo)
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

        if (_isThree)
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

        if (_isFour)
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

        if (_isFive)
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

        if (_isSix)
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
        //_tutorialObject[num].SetActive(true);
        //_tutorialButton[num].interactable = false;
        //ExitButton();
    }

    private void OnButtons()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            _tutorialButton[i].interactable = true;
        }
    }

    private void ExitButton()
    {
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialButton[6].interactable = true;
                return;
            }
        }
    }

    private void ClickExitButton()
    {
        _lobbyChanger.ChangeLobby(Defines.ESceneNumber.FantasyLobby);
    }

    private void QuestReset()
    {
        _tutorialController.QuestAcceptEvent.Invoke(2);
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

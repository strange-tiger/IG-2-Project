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
    [SerializeField] private LobbyChanger _lobbyChanger;
    [SerializeField] private GameObject _image;

    private Action OnButtonAction;

    private bool _isButton;
    private bool _isOn = true;
    private bool _isQuest;
    public bool IsQuest { get { return _isQuest; } set { _isQuest = value; } }

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
            if (_tutorialController.DialogueNum == 5 && !_isQuest)
            {
                _questText.text = "발판 7개를 모두 밟아보세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 6)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isOne = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (_isTwo)
        {
            if (_tutorialController.DialogueNum == 8 && !_isQuest)
            {
                _questText.text = "공을 그랩으로 집어서 골대에 넣어보세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 10)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isTwo = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (_isThree)
        {
            if (_tutorialController.DialogueNum == 16 && !_isQuest)
            {
                _questText.text = "마법봉을 주워서 마법을 사용하세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 17)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isThree = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (_isFour)
        {
            if (_tutorialController.DialogueNum == 23 && !_isQuest)
            {
                _questText.text = "음식을 먹고 포만감을 최대 수치까지 채워주세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 24)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isFour = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (_isFive)
        {
            if (_tutorialController.DialogueNum == 35 && !_isQuest)
            {
                _questText.text = "채광에 성공하여 골드를 획득하세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 36)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isFive = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (_isSix)
        {
            if (_tutorialController.DialogueNum == 45 && !_isQuest)
            {
                _questText.text = "무기를 집어 이시고르에게 돌아가세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 47 && !_isQuest)
            {
                _questText.text = "물건을 4개 베세요";
                _tutorialController.IsTutorialQuest = true;
            }

            if (_tutorialController.DialogueNum == 54)
            {
                _tutorialController.QuestAcceptEvent.Invoke(2);
                _isButton = false;
                _isOn = true;
                _isSix = false;
                _questText.text = null;
                _tutorialController.IsTutorialQuest = false;
                QuestReset();
            }
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0 || OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0)
        {
            _image.SetActive(false);
        }
        else
        {
            _image.SetActive(true);
        }
    }

    private void OnClickButton(int num)
    {
        //for (int i = 0; i < _tutorialObject.Length; ++i)
        //{
        //    if (_tutorialObject[i].activeSelf)
        //    {
        //        _isOn = false;
        //        return;
        //    }
        //}

        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)   
            {
                _isOn = false;
                return;
            }
        }

        if (_isOn)
        {
            switch (num)
            {
                case 0:
                    _tutorialController.QuestAcceptEvent.Invoke(3);
                    _isOne = true;
                    break;
                case 1:
                    _tutorialController.QuestAcceptEvent.Invoke(6);
                    _isTwo = true;
                    break;
                case 2:
                    _tutorialController.QuestAcceptEvent.Invoke(10);
                    _isThree = true;
                    break;
                case 3:
                    _tutorialController.QuestAcceptEvent.Invoke(17);
                    _isFour = true;
                    break;
                case 4:
                    _tutorialController.QuestAcceptEvent.Invoke(24);
                    _isFive = true;
                    break;
                case 5:
                    _tutorialController.QuestAcceptEvent.Invoke(36);
                    _isSix = true;
                    break;
                default:
                    break;
            }

            if (_tutorialObject[num].activeSelf)
            {
                _tutorialObject[num].SetActive(false);
                _tutorialObject[num].SetActive(true);
            }


            //_tutorialObject[num].SetActive(true);
            //_tutorialButton[num].interactable = false;
            ExitButton();
        }

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
        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialObject[i].activeSelf)
            {
                _tutorialObject[i].SetActive(false);
            }
        }

        for (int i = 0; i < _tutorialObject.Length; ++i)
        {
            if (_tutorialButton[i].enabled == false)
            {
                _tutorialButton[i].enabled = true;
            }
        }

        _tutorialController.QuestAcceptEvent.Invoke(2);

        if (_questText.text != null)
        {
            _questText.text = null;
        }
        _tutorialController.IsTutorialQuest = false;
        _isOn = true;
        _isQuest = false;
    }

    private void OnDisable()
    {
        _tutorialButton[6].onClick.RemoveListener(ClickExitButton);
    }
}

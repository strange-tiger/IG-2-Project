using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

using SceneType = Defines.ESceneNumber;


public class TutorialBettingUI : MonoBehaviourPun
{

    public UnityEvent OnTriggered = new UnityEvent();

    [Header("Betting Panel")]
    [SerializeField] private GameObject _bettingPanel;
    [SerializeField] private GameObject _tutorialConvasationPanel;

    [Header("Betting Button")]
    [SerializeField] private Button _betChampionOneButton;
    [SerializeField] private Button _betChampionTwoButton;
    [SerializeField] private Button _betChampionThreeButton;
    [SerializeField] private Button _betChampionFourButton;

    [Header("Betting Cancel Button")]
    [SerializeField] private Button _betCancelChampionOneButton;
    [SerializeField] private Button _betCancelChampionTwoButton;
    [SerializeField] private Button _betCancelChampionThreeButton;
    [SerializeField] private Button _betCancelChampionFourButton;

    [Header("Betting PopUp Panel")]
    [SerializeField] private GameObject _popUpPanel;
    [SerializeField] private TextMeshProUGUI _popUpMessage;
    [SerializeField] private Button _popUpOffButton;

    [Header("Skip PopUp Panel")]
    [SerializeField] private GameObject _skipPopUpPanel;
    [SerializeField] private Button _skipTutorialButton;
    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _cancelSkipButton;

    [Header("Betting InputField")]
    [SerializeField] private TMP_InputField[] _betChampionInputField;

    [Header("Betting Rate")]
    [SerializeField] private TextMeshProUGUI[] _betRateText;

    private bool _isBettingComplete;

    private void OnEnable()
    {

        _skipTutorialButton.onClick.RemoveListener(SkipPopUpOn);
        _skipTutorialButton.onClick.AddListener(SkipPopUpOn);

        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionOneButton.onClick.AddListener(BetChampionOne);

        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionTwoButton.onClick.AddListener(BetChampionTwo);

        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionThreeButton.onClick.AddListener(BetChampionThree);

        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
        _betChampionFourButton.onClick.AddListener(BetChampionFour);

        _betCancelChampionOneButton.onClick.RemoveListener(BetCancelChampionOne);
        _betCancelChampionOneButton.onClick.AddListener(BetCancelChampionOne);

        _betCancelChampionTwoButton.onClick.RemoveListener(BetCancelChampionTwo);
        _betCancelChampionTwoButton.onClick.AddListener(BetCancelChampionTwo);

        _betCancelChampionThreeButton.onClick.RemoveListener(BetCancelChampionThree);
        _betCancelChampionThreeButton.onClick.AddListener(BetCancelChampionThree);

        _betCancelChampionFourButton.onClick.RemoveListener(BetCancelChampionFour);
        _betCancelChampionFourButton.onClick.AddListener(BetCancelChampionFour);

        _skipButton.onClick.RemoveListener(SkipTutorial);
        _skipButton.onClick.AddListener(SkipTutorial);

        _cancelSkipButton.onClick.RemoveListener(CancelSkipTutorial);
        _cancelSkipButton.onClick.AddListener(CancelSkipTutorial);

        _popUpOffButton.onClick.RemoveListener(PopUpPanelOff);
        _popUpOffButton.onClick.AddListener(PopUpPanelOff);



        foreach (TMP_InputField inputfield in _betChampionInputField)
        {
            inputfield.onSelect.AddListener((string temp) =>
            {
                KeyboardManager.OpenKeyboard(KeyboardManager.EKeyboardLayout.NUMPAD);
            }
            );
        }
    }

    private void PopUpPanelOff()
    {
        _popUpPanel.SetActive(false);
        if (_isBettingComplete)
        {
            OnTriggered.Invoke();
        }
    }

    public void BettingPanelOn()
    {
        _bettingPanel.SetActive(true);
    }
    private void SkipPopUpOn()
    {
        _skipPopUpPanel.SetActive(true);
    }

    public void BettingPanelOff()
    {
        _bettingPanel.SetActive(false);
    }

    private void InputFieldClear()
    {
        for (int i = 0; i < _betChampionInputField.Length; ++i)
        {
            _betChampionInputField[i].text = null;
        }
    }

    private void BetChampion(int index)
    {
        if (_betChampionInputField[index].text != null)
        {
            InputFieldClear();

            _popUpPanel.SetActive(true);

            _popUpMessage.text = "베팅이 완료되었습니다.";

            _betRateText[index].text = "100";

            _isBettingComplete = true;
        }
    }

    private void BetChampionOne()
    {
        BetChampion(0);
    }

    private void BetChampionTwo()
    {
        BetChampion(1);
    }

    private void BetChampionThree()
    {
        BetChampion(2);
    }

    private void BetChampionFour()
    {
        BetChampion(3);
    }

    private void BetCancelChampionOne()
    {
        _popUpPanel.SetActive(true);
        _popUpMessage.text = "베팅 내역이 없습니다.";
    }

    private void BetCancelChampionTwo()
    {
        _popUpPanel.SetActive(true);
        _popUpMessage.text = "베팅 내역이 없습니다.";
    }

    private void BetCancelChampionThree()
    {
        _popUpPanel.SetActive(true);
        _popUpMessage.text = "베팅 내역이 없습니다.";
    }

    private void BetCancelChampionFour()
    {

        _popUpPanel.SetActive(true);
        _popUpMessage.text = "베팅 내역이 없습니다.";

    }

       
    private void SkipTutorial() => SceneManager.LoadScene((int)SceneType.ArenaRoom);
        
    private void CancelSkipTutorial() => _skipPopUpPanel.SetActive(false);

    private void OnDisable()
    {
        _skipTutorialButton.onClick.RemoveListener(SkipPopUpOn);
        _betChampionOneButton.onClick.RemoveListener(BetChampionOne);
        _betChampionTwoButton.onClick.RemoveListener(BetChampionTwo);
        _betChampionThreeButton.onClick.RemoveListener(BetChampionThree);
        _betChampionFourButton.onClick.RemoveListener(BetChampionFour);
        _betCancelChampionOneButton.onClick.RemoveListener(BetCancelChampionOne);
        _betCancelChampionTwoButton.onClick.RemoveListener(BetCancelChampionTwo);
        _betCancelChampionThreeButton.onClick.RemoveListener(BetCancelChampionThree);
        _betCancelChampionFourButton.onClick.RemoveListener(BetCancelChampionFour);
        _popUpOffButton.onClick.RemoveListener(PopUpPanelOff);
        _skipButton.onClick.RemoveListener(SkipTutorial);
        _cancelSkipButton.onClick.RemoveListener(CancelSkipTutorial);


    }
}

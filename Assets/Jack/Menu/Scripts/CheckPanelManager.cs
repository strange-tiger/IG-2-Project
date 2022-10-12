using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CheckPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject _checkPanel;
    [SerializeField] private TextMeshProUGUI _informationText;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _refuseButton;

    /// <summary>
    /// 확인 패널에서 동작할 함수를 저장할 delegate
    /// </summary>
    public delegate void CheckButtonAction();

    /// <summary>
    /// 확인 패널을 띄우는 함수
    /// </summary>
    /// <param name="informationMessage"> 확인 메시지 </param>
    /// <param name="acceptAction">확인 시 동작할 함수 void()</param>
    /// <param name="refuseAction">취소 시 동작할 함수 void()</param>
    public void SetActiveCheckPanel(string informationMessage, 
        CheckButtonAction acceptAction, CheckButtonAction refuseAction)
    {
        _informationText.text = informationMessage;
        
        _acceptButton.onClick.RemoveAllListeners();
        _acceptButton.onClick.AddListener(() => 
        { 
            acceptAction();
            _checkPanel.SetActive(false);
        });

        _refuseButton.onClick.RemoveAllListeners();
        _refuseButton.onClick.AddListener(() => 
        { 
            refuseAction();
            _checkPanel.SetActive(false); 
        });

        _checkPanel.SetActive(true);
    }
}

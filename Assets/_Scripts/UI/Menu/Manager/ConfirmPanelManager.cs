using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject _confirmPanel;
    private TextMeshProUGUI _confirmMessageText;

    private void Awake()
    {
        _confirmMessageText = _confirmPanel.GetComponentInChildren<TextMeshProUGUI>();
        _confirmPanel.SetActive(false);
    }

    public void ShowConfirmPanel(string confirmMessage)
    {
        _confirmMessageText.text = confirmMessage;
        _confirmPanel.SetActive(true);
    }
}

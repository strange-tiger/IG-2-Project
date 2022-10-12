using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendPanelManager : MonoBehaviour
{
    [SerializeField] private GameObject _blockPanel;
    [SerializeField] private Button _userBlockButton;
    private TextMeshProUGUI _targetUserName;

    private void Awake()
    {
        _blockPanel.SetActive(false);
        _targetUserName = GetComponentInChildren<TextMeshProUGUI>();
        _userBlockButton.onClick.AddListener(() => { BlockUser(); });
    }

    private void BlockUser()
    {
        _blockPanel.SetActive(true);
        _blockPanel.GetComponentInChildren<TextMeshProUGUI>().text = _targetUserName.text + "님을 차단하였습니다.";
        Debug.Log(_targetUserName.text);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddFriendButton : NeedCheckButton
{
    [SerializeField] private GameObject _friendPanel;
    private TextMeshProUGUI _targetUserName;

    private void Awake()
    {
        _targetUserName = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
        _checkMessage = _targetUserName.text + _checkMessage;
        Debug.Log(_checkMessage);
    }

    protected override void AcceptAction()
    {
        Debug.Log($"Request Friend to {_targetUserName.text}");
        _friendPanel.SetActive(false);
    }

    protected override void RefuseAction()
    {
        base.RefuseAction();
    }
}
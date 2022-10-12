using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddFriendButton : NeedCheckButton
{
    private TextMeshProUGUI _targetUserName;

    private void Awake()
    {
        _targetUserName = transform.parent.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void AcceptAction()
    {
        Debug.Log($"Request Friend to {_targetUserName.text}");
        
    }
}
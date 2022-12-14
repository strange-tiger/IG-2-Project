using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public abstract class NeedCheckButton : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// 확인 시 나올 메시지
    /// </summary>
    [SerializeField] private string _checkMessage;

    /// <summary>
    /// Check Panel을 띄울 메니저
    /// </summary>
    private static CheckPanelManager _checkPanelManager;

    private void Start()
    {
        if(!_checkPanelManager)
        {
            _checkPanelManager = FindObjectOfType<CheckPanelManager>();
        }

        Button myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() =>
        {
            _checkPanelManager.ShowCheckPanel(_checkMessage, AcceptAction, RefuseAction);
        });
    }

    /// <summary>
    /// 확인 시 할 동작을 지정
    /// </summary>
    protected abstract void AcceptAction();

    /// <summary>
    /// 취소 시 할 동작을 지정
    /// </summary>
    protected virtual void RefuseAction()
    {
        Debug.Log("Refused");
    }
}

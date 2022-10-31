using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public abstract class NeedCheckButton : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Ȯ�� �� ���� �޽���
    /// </summary>
    [SerializeField] private string _checkMessage;

    /// <summary>
    /// Check Panel�� ��� �޴���
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
    /// Ȯ�� �� �� ������ ����
    /// </summary>
    protected abstract void AcceptAction();

    /// <summary>
    /// ��� �� �� ������ ����
    /// </summary>
    protected virtual void RefuseAction()
    {
        Debug.Log("Refused");
    }
}

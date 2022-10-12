using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class NeedCheckButton : MonoBehaviour
{
    /// <summary>
    /// Ȯ�� �� ���� �޽���
    /// </summary>
    [SerializeField] protected string _checkMessage;

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
            _checkPanelManager.SetActiveCheckPanel(_checkMessage, AcceptAction, RefuseAction);
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

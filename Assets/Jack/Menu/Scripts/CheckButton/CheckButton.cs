using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CheckButton : MonoBehaviour
{
    [SerializeField] private string _checkMessage;
    public string Message { get { return _checkMessage; } }

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
            _checkPanelManager.CheckPanelSetting(Message, AcceptAction, RefuseAction);
        });
    }

    public abstract void AcceptAction();
    public virtual void RefuseAction()
    {
        Debug.Log("Refused");
    }
}

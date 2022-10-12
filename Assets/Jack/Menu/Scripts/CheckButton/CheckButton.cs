using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheckButton 
{
    [SerializeField] private string _checkMessage;
    public string Message { get; }

    public abstract void AcceptAction();
    public virtual void RefuseAction()
    {
        Debug.Log("Refused");
    }
}

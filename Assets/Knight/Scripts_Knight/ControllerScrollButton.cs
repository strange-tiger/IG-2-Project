using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using _Switch = Defines.ESwitchController;

public class ControllerScrollButton : MonoBehaviour
{
    public UnityEvent<bool> SwitchController = new UnityEvent<bool>();

    private SwitchControllerScrollUI _switchControllerScrollUI;
    private _Switch _type = _Switch.Left;
    // private Dictionary<_Switch, ControllerTypeDelegate> _controllerTable = new Dictionary<_Switch, ControllerTypeDelegate>();

    public _Switch Type
    {
        get
        {
            return _type;
        }
        private set
        {
            _type = value;
            _switchControllerScrollUI.TextOutput(_type);
        }
    }

    private delegate void ControllerTypeDelegate();

    private void Awake()
    {
        _switchControllerScrollUI = GetComponent<SwitchControllerScrollUI>();
        Type = _Switch.Left;

        //_controllerTable.Add(_Switch.Left, ControllerTypeLeft);
        //_controllerTable.Add(_Switch.Right, ControllerTypeRight);
    }

    public void OnClickLeftButton()
    {
        if (Type > _Switch.Left)
        {
            SwitchController.Invoke(true);
            --Type;
        }
    }

    public void OnClickRightButton()
    {
        if (Type < _Switch.Right)
        {
            SwitchController.Invoke(false);
            ++Type;
        }
    }

    #region Legacy
    /*
    private void OnClickLeftButton()
    {
        if (Type - 1 < _Switch.Left)
        {
            return;
        }
        --Type;
        _controllerTable[Type].Invoke();
    }

    private void OnClickRightButton()
    {
        if (Type + 1 >= _Switch.End)
        {
            return;
        }
        ++Type;
        _controllerTable[Type].Invoke();
    }

    private void ControllerTypeLeft()
    {
        Debug.Log("왼쪽으로");
        SwitchController.Invoke(false);
    }

    private void ControllerTypeRight()
    {
        Debug.Log("오른쪽으로");
        SwitchController.Invoke(true);
    }
    */
    #endregion
}

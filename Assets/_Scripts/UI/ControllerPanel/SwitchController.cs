using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using _Controller = Defines.ESwitchController;


public class SwitchController : MonoBehaviour
{
    public UnityEvent<bool> SwitchControllerEvent = new UnityEvent<bool>();

    private SwitchControllerScrollUI _switchControllerScrollUI;
    private _Controller _type = _Controller.Left;
    // private Dictionary<_Switch, ControllerTypeDelegate> _controllerTable = new Dictionary<_Switch, ControllerTypeDelegate>();

    public _Controller Type
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
        Type = _Controller.Left;

        //_controllerTable.Add(_Switch.Left, ControllerTypeLeft);
        //_controllerTable.Add(_Switch.Right, ControllerTypeRight);
    }

    public void OnClickLeftButton()
    {
        if (Type > _Controller.Left)
        {
            SwitchControllerEvent.Invoke(true);
            --Type;
        }
    }

    public void OnClickRightButton()
    {
        if (Type < _Controller.Right)
        {
            SwitchControllerEvent.Invoke(false);
            ++Type;
        }
    }
}

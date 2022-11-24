using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using System;
using _Controller = Defines.ESwitchController;


public class SwitchController : MonoBehaviourPun
{
    public UnityEvent<bool> SwitchControllerEvent = new UnityEvent<bool>();

    private ControlScrollUI _controllerScrollUI;
    private _Controller _type = _Controller.Left;

    private bool _isControllerRight;

    public _Controller Type
    {
        get
        {
            return _type;
        }
        private set
        {
            _type = value;
            _controllerScrollUI.TextOutput(_type);
        }
    }

    private void Awake()
    {
        _controllerScrollUI = GetComponent<ControlScrollUI>();

        if (PlayerPrefs.HasKey("WhatIsTheMainController"))
        {
            _isControllerRight = Convert.ToBoolean(PlayerPrefs.GetInt("WhatIsTheMainController"));
        }

        if (_isControllerRight)
        {
            Type = _Controller.Right;
        }
        else
        {
            Type = _Controller.Left;
        }
    }

    public void OnClickLeftButton()
    {
        if (Type > _Controller.Left)
        {
            SwitchControllerEvent.Invoke(false);
            --Type;
        }
    }

    public void OnClickRightButton()
    {
        if (Type < _Controller.Right)
        {
            SwitchControllerEvent.Invoke(true);
            ++Type;
        }
    }
}

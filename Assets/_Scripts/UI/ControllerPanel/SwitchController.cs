using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using _Controller = Defines.ESwitchController;


public class SwitchController : MonoBehaviourPun
{
    public UnityEvent<bool> SwitchControllerEvent = new UnityEvent<bool>();

    private ControlScrollUI _controllerScrollUI;
    private _Controller _type = _Controller.Left;

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

    private delegate void ControllerTypeDelegate();

    private void Awake()
    {
        _controllerScrollUI = GetComponent<ControlScrollUI>();
        Type = _Controller.Left;
    }

    public void OnClickLeftButton()
    {
        if (Type > _Controller.Left)
        {
            SwitchControllerEvent.Invoke(false);
            --Type;
            Debug.Log("왼쪽버튼클릭");
        }
    }

    public void OnClickRightButton()
    {
        if (Type < _Controller.Right)
        {
            SwitchControllerEvent.Invoke(true);
            ++Type;
            Debug.Log("오른쪽버튼클릭");
        }
    }
}

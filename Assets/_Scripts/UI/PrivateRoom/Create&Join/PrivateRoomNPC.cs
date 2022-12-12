using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.EPrivateRoomUIIndex;

public class PrivateRoomNPC : InteracterableObject
{
    [SerializeField] PrivateRoomUIManager _privateRoomUI;

    /// <summary>
    /// OutFocus를 호출해 상호작용이 불가하게 한다.
    /// 사설 공간 UI의 방 참가 UI를 띄운다.
    /// </summary>
    public override void Interact()
    {
        base.Interact();

        OutFocus();

        _privateRoomUI.LoadUI(_UI.JOIN);
    }
}

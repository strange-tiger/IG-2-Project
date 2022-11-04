using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.EPrivateRoomUIIndex;

public class PrivateRoomNPC : InteracterableObject
{
    [SerializeField] PrivateRoomUIManager _privateRoomUI;

    public override void Interact()
    {
        base.Interact();

        _privateRoomUI.LoadUI(_UI.JOIN);
    }
}

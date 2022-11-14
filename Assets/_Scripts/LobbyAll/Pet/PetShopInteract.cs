using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

using _UI = Defines.EPetUIIndex;

public class PetShopInteract : InteracterableObject
{
    [SerializeField] PetUIManager _petUI;

    public override void Interact()
    {
        base.Interact();

        OutFocus();

        _petUI.LoadUI(_UI.POPUP);
    }
}

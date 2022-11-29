using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using _UI = Defines.EPetShopUIIndex;

public class PetShopInteract : InteracterableObject
{
    [SerializeField] PetShopUIManager _petUI;

    public override void Interact()
    {
        base.Interact();

        OutFocus();

        _petUI.LoadUI(_UI.CONVERSATION);
    }
}

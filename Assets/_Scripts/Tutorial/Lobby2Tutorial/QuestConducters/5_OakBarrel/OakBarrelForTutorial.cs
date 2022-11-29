using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrelForTutorial : InteracterableObject
{
    public override void Interact()
    {
        base.Interact();
        gameObject.SetActive(false);

        OakBarrelInteractionForTutorial _interaction
             = PlayerControlManager.Instance.transform.root.GetComponentInChildren<OakBarrelInteractionForTutorial>();
        _interaction.GetInToOakBarrel();
    }
}

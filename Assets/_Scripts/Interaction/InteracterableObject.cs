using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracterableObject : FocusableObjects
{
    public virtual void Interact()
    {
        Debug.Log(gameObject.name + ": interact");
    }
}

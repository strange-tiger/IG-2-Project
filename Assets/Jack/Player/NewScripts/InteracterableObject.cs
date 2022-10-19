using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteracterableObject : FocusableObjects
{
    public void Interact()
    {
        Debug.Log(gameObject.name + ": interact");
    }
}

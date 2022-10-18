using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    public void OnFocus();
    public void OutFocus();

    public void Interact();
}

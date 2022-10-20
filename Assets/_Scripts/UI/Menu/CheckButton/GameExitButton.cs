using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitButton : NeedCheckButton
{
    protected override void AcceptAction()
    {
        Debug.Log("Game Exit");
        Application.Quit();
    }
}

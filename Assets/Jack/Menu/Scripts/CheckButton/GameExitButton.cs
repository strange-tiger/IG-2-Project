using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExitButton : CheckButton
{
    public override void AcceptAction()
    {
        Debug.Log("Game Exit");
        Application.Quit();
    }
}

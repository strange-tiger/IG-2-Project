using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrivateRoomUI : InteracterableObject
{
    [SerializeField] GameObject _ui;

    public override void Interact()
    {
        _ui.SetActive(true);
    }
}

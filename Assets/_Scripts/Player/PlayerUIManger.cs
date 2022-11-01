using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManger : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(_input.IsInventoryOn)
        {
            MenuUIManager.Instance.ShowInventory();
        }
    }
}

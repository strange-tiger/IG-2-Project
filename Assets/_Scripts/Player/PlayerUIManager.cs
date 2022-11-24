using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(!MenuUIManager.Instance.IsUIOn && _input.IsInventoryOn)
        {
            MenuUIManager.Instance.ShowMenu();
        }
        else if (MenuUIManager.Instance.IsUIOn && _input.IsInventoryOn)
        {
            MenuUIManager.Instance.HideMenu();
        }
    }
}

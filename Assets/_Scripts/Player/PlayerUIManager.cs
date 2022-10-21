using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    private PlayerInput _input;
    [SerializeField] private PlayerMenuUIManager _menuUIManager;

    public bool IsMoveable { get; private set; }
    public bool IsRayable { get; private set; }

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(_input.InputStart)
        {
            _menuUIManager.ShowMenu();
        }
    }
}

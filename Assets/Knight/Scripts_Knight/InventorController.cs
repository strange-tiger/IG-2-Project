using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorController : MonoBehaviour
{
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _canvas;

    void Start()
    {
        _canvas = GameObject.Find("SettingCanvas");
    }

    void Update()
    {
        if (_playerInput.IsInventoryOn)
        {
            _canvas.SetActive(true);
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorController : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _canvas;

    void Start()
    {
        
    }

    void Update()
    {
        if (_playerInput.IsInventoryOn)
        {
            _canvas.SetActive(true);
        }
        
    }
}

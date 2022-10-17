using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InventorController : MonoBehaviourPun
{
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _canvas;

    void Start()
    {
        _canvas = GameObject.Find("SettingCanvas");
        _playerInput = gameObject.GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_playerInput.IsInventoryOn)
        {
            _canvas.SetActive(true);
        }
        
    }
}

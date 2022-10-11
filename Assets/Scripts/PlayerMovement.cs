using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _controller;
    private Transform _cam;

    private float _speed = 3.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _cam = Camera.main.GetComponent<Transform>();
    }

    private void Update()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        if (_playerInput.IsMove)
        {
            Vector3 playerLook = _cam.TransformDirection(Vector3.forward);

            _controller.SimpleMove(playerLook * _speed);
        }
    }
}

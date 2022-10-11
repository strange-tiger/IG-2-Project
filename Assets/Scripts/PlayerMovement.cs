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
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            float inputX = thumbstick.x * _speed * Time.deltaTime;
            float inputZ = thumbstick.y * _speed * Time.deltaTime;

            Vector3 playerLook = _cam.TransformDirection(Vector3.forward);

            _controller.Move(new Vector3(inputX, 0f, inputZ) + playerLook);
        }
    }
}

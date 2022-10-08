using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _controller;

    private float _speed = 3.0f;
    private float _rotateSpeed = 2.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }
    private void MovePlayer()
    {
        if (_playerInput.IsMove)
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            float inputX = thumbstick.x * _speed * Time.deltaTime;
            float inputZ = thumbstick.y * _speed * Time.deltaTime;

            _controller.Move(new Vector3(inputX, 0f, inputZ));
        }
    }

    private void RotatePlayer()
    {
        if (_playerInput.IsRotate)
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            transform.Rotate(thumbstick * _rotateSpeed);

            //if (thumbstick.x < 0)
            //{
            //    gameObject.transform.Rotate(0, thumbstick.x * 2, 0);
            //}
            //else if (thumbstick.x > 0)
            //{
            //    gameObject.transform.Rotate(0, thumbstick.x * 2, 0);
            //}

            //if (thumbstick.y < 0)
            //{
            //    gameObject.transform.Rotate(0, thumbstick.y, 0);
            //}
            //else if (thumbstick.y > 0)
            //{
            //    gameObject.transform.Rotate(0, thumbstick.y, 0);
            //}
        }
    }
}

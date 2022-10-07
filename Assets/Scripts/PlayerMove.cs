using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // private PlayerInput _playerInput;
    private CharacterController _controller;

    public float speed = 3.0f;

    private void Start()
    {
        // _playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {

            // Rotate around y - axis
            // transform.Rotate(0, _playerInput.InputX * rotateSpeed, 0);

            // Move forward / backward
            // Vector3 forward = transform.TransformDirection(Vector3.forward);
            //Vector3 leftAndRight = 

            // float moveX = _playerInput.InputX * speed * Time.deltaTime;
            // float moveZ = _playerInput.InputZ * speed * Time.deltaTime;
            //float curSpeed = speed * _playerInput.InputZ;
            //_controller.SimpleMove(forward * curSpeed);
            // _controller.Move(new Vector3( moveX, 0f, moveZ));

            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            float inputX = thumbstick.x * speed * Time.deltaTime;
            float inputZ = thumbstick.y * speed * Time.deltaTime;

            _controller.Move(new Vector3(inputX, 0f, inputZ));
        }
    }
}

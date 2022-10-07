using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float speed = 3.0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
    }
    private void MovePlayer()
    {
        if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

            float inputX = thumbstick.x * speed * Time.deltaTime;
            float inputZ = thumbstick.y * speed * Time.deltaTime;

            _controller.Move(new Vector3(inputX, 0f, inputZ));
        }
    }

    private void RotatePlayer()
    {
        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            Vector2 thumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

            if (thumbstick.x < 0)
            {
                gameObject.transform.Rotate(0, thumbstick.x * 2, 0);
            }
            else if (thumbstick.x > 0)
            {
                gameObject.transform.Rotate(0, thumbstick.x * 2, 0);
            }

            if (thumbstick.y < 0)
            {
                gameObject.transform.Rotate(0, thumbstick.y, 0);
            }
            else if (thumbstick.y > 0)
            {
                gameObject.transform.Rotate(0, thumbstick.y, 0);
            }
        }
    }
}

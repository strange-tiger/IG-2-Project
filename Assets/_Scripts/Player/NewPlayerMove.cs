using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMove : MonoBehaviour
{
    private CharacterController _playerController;
    private SwitchController _switchController;

    [Header("플레이어의 기본 이동속도")]
    [SerializeField] private float _movementSpeed = 2f;

    [Header("플레이어의 속도 조정을 위한 것")]
    public float MoveScale;

    private Vector3 _playerVelocity;
    private bool _isgroundedPlayer;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _switchController = GetComponentInChildren<SwitchController>();

        MoveScale = 1f;
    }

    void Update()
    {
        _isgroundedPlayer = _playerController.isGrounded;

        if (_isgroundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        if (_switchController.Type == 0)
        {
            PlayerMovement(OVRInput.Touch.PrimaryThumbstick, OVRInput.Axis2D.PrimaryThumbstick);
            PlayerRotate(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
        }
        else if (_switchController.Type != 0)
        {
            PlayerMovement(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
            PlayerRotate(OVRInput.Touch.PrimaryThumbstick, OVRInput.Axis2D.PrimaryThumbstick);
        }
    }

    private void PlayerMovement(OVRInput.Touch value, OVRInput.Axis2D stick)
    {
        if (OVRInput.Get(value))
        {
            Vector2 thumbstick = OVRInput.Get(stick);

            Vector3 playerMove = new Vector3(thumbstick.x, 0f, thumbstick.y);

            _playerController.Move(playerMove * Time.deltaTime * _movementSpeed * MoveScale);
        }
    }

    private void PlayerRotate(OVRInput.Touch value, OVRInput.Axis2D stick)
    {
        if (OVRInput.Get(value))
        {
            Vector2 thumbstick = OVRInput.Get(stick);

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

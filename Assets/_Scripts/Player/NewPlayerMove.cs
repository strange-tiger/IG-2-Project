using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMove : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterController _playerController;
    private SwitchController _switchController;

    [Header("플레이어의 기본 이동속도")]
    [SerializeField] private float _movementSpeed = 2f;

    [Header("플레이어의 속도 조정을 위한 것")]
    public float MoveScale;

    private Vector3 _playerVelocity;
    private bool _isgroundedPlayer;
    private bool _isMoveControllerRight;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<CharacterController>();
        _switchController = GetComponent<SwitchController>();

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
            LookAround(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
        }
        else if (_switchController.Type == 0)
        {
            PlayerMovement(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
            LookAround(OVRInput.Touch.PrimaryThumbstick, OVRInput.Axis2D.PrimaryThumbstick);
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

    private void LookAround(OVRInput.Touch value, OVRInput.Axis2D stick)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ball : MonoBehaviourPunCallbacks
{
    private Vector3 _ballPosition;

    [SerializeField]
    private float _resetBallTimer;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _currentTime;
    private readonly float _thrust = 1.15f;

    private bool _isGrabBall;

    private void Awake()
    {
        _ballPosition = transform.position;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        SetBall();

        if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) && _isGrabBall == true)
        {
            _rigidbody.AddForce(0, _thrust, 0, ForceMode.Impulse);
        }
    }

    private void SetBall()
    {
        if (_rigidbody.velocity == Vector3.zero)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime > _resetBallTimer)
            {
                transform.position = _ballPosition;
                _currentTime -= _currentTime;
            }
        }
        else
        {
            if (_currentTime != 0f)
            {
                _currentTime -= _currentTime;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isGrabBall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isGrabBall = false;
        }
    }
}


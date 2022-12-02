using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ball : MonoBehaviourPunCallbacks
{
    private Vector3 _ballPosition;

    [SerializeField] private float _resetBallTimer;

    private ThrowBall _ThrowBall;
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;
    private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    private float _ballNoTouchTime;

    private void Awake()
    {
        _ballPosition = transform.position;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _syncOVRDistanceGrabbable = GetComponent<SyncOVRDistanceGrabbable>();
        _ThrowBall = GetComponent<ThrowBall>();
    }

    private void Update()
    {
        SetBall();

        if (_syncOVRDistanceGrabbable.isGrabbed == true)
        {
            _ThrowBall.enabled = false;
        }
        else if (_syncOVRDistanceGrabbable.isGrabbed == false)
        {
            _ThrowBall.enabled = true;
        }
    }

    /// <summary>
    /// Ball에 아무런 움직임이 없을 때 초기위치로
    /// </summary>
    private void SetBall()
    {
        if (_rigidbody.velocity == Vector3.zero)
        {
            _ballNoTouchTime += Time.deltaTime;

            if (_ballNoTouchTime > _resetBallTimer)
            {
                transform.position = _ballPosition;
                _ballNoTouchTime -= _ballNoTouchTime;
            }
        }
        else
        {
            if (_ballNoTouchTime != 0f)
            {
                _ballNoTouchTime -= _ballNoTouchTime;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("BallGameCourtFloor"))
        {
            _audioSource.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BallDie")
        {
            transform.position = _ballPosition;
        }
    }

    public override void OnDisable()
    {
        transform.position = _ballPosition;
    }
}


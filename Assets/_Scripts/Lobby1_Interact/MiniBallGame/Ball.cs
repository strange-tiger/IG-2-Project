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
    private AudioSource _audioSource;

    private float _currentTime;
    private bool[] _isGrabBall = new bool[2];

    private void Awake()
    {
        _ballPosition = transform.position;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        SetBall();

        if ((OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger)) && _isGrabBall[0] == true || _isGrabBall[1] == true)
        {
            _rigidbody.AddForce(0, 40f, 20f);
        }

        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    _rigidbody.AddForce(0, 60f, 30f);
        //}
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
            if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
            {
                for (int i = 0; i < _isGrabBall.Length; ++i)
                {
                    if (_isGrabBall[i] != true)
                    {
                        _isGrabBall[i] = true;
                        break;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BallGameCourtFloor")
        {
            _audioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < _isGrabBall.Length; ++i)
            {
                if (_isGrabBall[i] != false)
                {
                    _isGrabBall[i] = false;
                    break;
                }
            }
        }
    }
}


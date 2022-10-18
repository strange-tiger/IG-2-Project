using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Ball : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Vector3 _ballPosition;

    [SerializeField]
    private float _resetBallTimer = 5.0f;

    private Rigidbody _rigidbody;
    private float _currentTime;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        SetBall();
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
}


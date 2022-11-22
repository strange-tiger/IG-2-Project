using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rigidbody.AddForce(0, 3, 1.5f, ForceMode.Impulse);
    }

    void Start()
    {
        this.enabled = false;
    }
}

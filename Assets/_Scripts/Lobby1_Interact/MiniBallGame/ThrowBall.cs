using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    private Vector3 _throwPower;

    private void Awake()
    {
        _throwPower = new Vector3(0f, 15f, 50f);
    }

    private void OnEnable()
    {
        _rigidbody.AddRelativeForce(_throwPower, ForceMode.Force);
    }

    void Start()
    {
        this.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedMovement : MonoBehaviour
{
    [SerializeField] private float _bounceForce = 2f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * _bounceForce, ForceMode.Impulse);
        Destroy(gameObject, 20f);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingObjectMovement : MonoBehaviour
{
    [Header("Basic Speed")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _destroyOffsetTime;
    private WaitForSeconds _waitForDestroy;

    private LayerMask _ShootingObjectLayer;
    private Collider[] _colliders;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _waitForDestroy = new WaitForSeconds(_destroyOffsetTime);

        _ShootingObjectLayer = LayerMask.NameToLayer("ShootingObject");

        _colliders = GetComponentsInChildren<Collider>();

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * _moveSpeed;
        _rigidbody.AddTorque(transform.forward * _rotationSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShootingHitRange"))
        {
            gameObject.layer = _ShootingObjectLayer;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ShootingHitRange"))
        {
            foreach (Collider collider in _colliders)
            {
                collider.enabled = false;
            }
            StartCoroutine(CoDestroyObject());
        }
    }

    private IEnumerator CoDestroyObject()
    {
        yield return _waitForDestroy;
        Destroy(gameObject);
    }
}

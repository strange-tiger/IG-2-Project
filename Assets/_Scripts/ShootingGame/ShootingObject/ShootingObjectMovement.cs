using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootingObjectMovement : MonoBehaviourPun
{
    [Header("Basic Speed")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _destroyOffsetTime;
    private WaitForSeconds _waitForDestroy;

    private LayerMask _breakableObjectLayer;
    private LayerMask _unbreakableObjectLayer;

    private Rigidbody _rigidbody;
    private ShootingObjectHealth _health;

    private void Awake()
    {
        _waitForDestroy = new WaitForSeconds(_destroyOffsetTime);

        _breakableObjectLayer = LayerMask.NameToLayer("BreakableShootingObject");
        _unbreakableObjectLayer = LayerMask.NameToLayer("UnbreakableShootingObject");
        gameObject.layer = _unbreakableObjectLayer;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * _moveSpeed;
        _rigidbody.AddTorque(transform.forward * _rotationSpeed, ForceMode.Impulse);

        _health = GetComponent<ShootingObjectHealth>();
        _health.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("ShootingFloor"))
        {
            StartCoroutine(CoDestroyObject());
            gameObject.layer = _unbreakableObjectLayer;
            _health.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShootingHitRange"))
        {
            StopAllCoroutines();
            gameObject.layer = _breakableObjectLayer;
            _health.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ShootingHitRange"))
        {
            StartCoroutine(CoDestroyObject());
            gameObject.layer = _unbreakableObjectLayer;
            _health.enabled = false;
        }
    }

    private IEnumerator CoDestroyObject()
    {
        yield return _waitForDestroy;
        //Destroy(gameObject);
        photonView.RPC("DestroySelf", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    private void DestroySelf()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

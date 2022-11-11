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

        _health = GetComponent<ShootingObjectHealth>();
        _health.enabled = false;

        if (PhotonNetwork.IsMasterClient)
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.velocity = transform.forward * _moveSpeed;
            _rigidbody.AddTorque(transform.forward * _rotationSpeed, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(collision.collider.CompareTag("ShootingFloor"))
        {
            StartCoroutine(CoDestroyObject());
            //gameObject.layer = _unbreakableObjectLayer;
            //_health.enabled = false;
            photonView.RPC("SetLayer", RpcTarget.AllViaServer, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.CompareTag("ShootingHitRange"))
        {
            StopAllCoroutines();
            //gameObject.layer = _breakableObjectLayer;
            //_health.enabled = true;
            photonView.RPC("SetLayer", RpcTarget.AllViaServer, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (other.CompareTag("ShootingHitRange"))
        {
            StartCoroutine(CoDestroyObject());
            //gameObject.layer = _unbreakableObjectLayer;
            //_health.enabled = false;
            photonView.RPC("SetLayer", RpcTarget.AllViaServer, false);
        }
    }

    [PunRPC]
    private void SetLayer(bool isBreakable)
    {
        gameObject.layer = isBreakable ? _breakableObjectLayer : _unbreakableObjectLayer;
        _health.enabled = isBreakable;
    }

    private IEnumerator CoDestroyObject()
    {
        yield return _waitForDestroy;
        PhotonNetwork.Destroy(gameObject);
        //Destroy(gameObject);
    }
}

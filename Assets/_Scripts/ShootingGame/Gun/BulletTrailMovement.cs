using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailMovement : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _lifeTime = 2f;
    private WaitForSeconds _waitForLifeTime;

    private readonly Vector3 ZERO_VECTOR = Vector3.zero;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _waitForLifeTime = new WaitForSeconds(_lifeTime);
    }

    private void Start()
    {
        transform.parent = null;
    }

    private void OnEnable()
    {
        ResetBulletTrail();
    }

    private void ResetBulletTrail()
    {
        StopAllCoroutines();
        _rigidbody.velocity = ZERO_VECTOR;
        _rigidbody.velocity = transform.forward * _bulletSpeed;
        StartCoroutine(CoDisableSelf());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("ShootingHitRange"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("ShootingHitRange"))
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator CoDisableSelf()
    {
        yield return _waitForLifeTime;
        gameObject.SetActive(false);
    }
}
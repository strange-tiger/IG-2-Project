using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomBulletTrail : MonoBehaviour
{
    [SerializeField]
    private WaitingRoomRevolver _revolver;

    [SerializeField] private float _bulletSpeed;

    [SerializeField] private float _fadeDistancePoint = 2f;
    [SerializeField] private float _fadeTime;

    private Color _originalColor = new Color();
    private Material _material;
    private readonly Vector3 ZERO_VECTOR = Vector3.zero;

    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ShootingHitRange"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ShootingHitRange"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _revolver.ReturnToBulletPull(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailMovement : MonoBehaviour
{
    private GunShoot _gun;

    [SerializeField] private float _bulletSpeed;

    [SerializeField] private float _fadeDistancePoint = 2f;
    [SerializeField] private float _fadeTime;

    private Color _originalColor = new Color();
    private Material _material;
    private readonly Vector3 ZERO_VECTOR = Vector3.zero;

    private float _elapsedTime;
    private bool _isOutRange;
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _gun = transform.root.GetComponentInChildren<GunShoot>();

        _rigidbody = GetComponent<Rigidbody>();

        //_material = GetComponent<Material>();
        //_originalColor = _material.color;

        _elapsedTime = 0f;
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
        //_material.color = _originalColor;
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
            //_isOutRange = true;
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeToDisable()
    {
        float alpha = _originalColor.a;

        while (true)
        {
            alpha = Mathf.Lerp(alpha, 0f, _fadeTime);
            _material.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, alpha);
            yield return null;
        }
    }

    private void OnDisable()
    {
        _gun.ReturnToBulletPull(gameObject);
    }
}
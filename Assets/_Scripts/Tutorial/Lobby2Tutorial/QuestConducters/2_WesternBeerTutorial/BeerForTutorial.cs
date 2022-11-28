using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerForTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _fullBeer;
    [SerializeField] private AudioClip _drinkSound;
    [SerializeField] private BoxCollider _grabCollider;
    [SerializeField] private BoxCollider _beerCollider;
    private BoxCollider _myCollider;

    private Vector3 _initBeerPosition;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _initBeerPosition = transform.position;
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        _myCollider.enabled = true;
        _rigidbody.useGravity = true;

        transform.position = _initBeerPosition;
        transform.rotation = Quaternion.identity;
        _fullBeer.SetActive(true);
        _grabCollider.enabled = true;
    }

    public void DrinkBeer()
    {
        _audioSource.PlayOneShot(_drinkSound);
        _fullBeer.SetActive(false);
        _grabCollider.enabled = false;
        _myCollider.enabled = false;
        _rigidbody.useGravity = false;
    }
}

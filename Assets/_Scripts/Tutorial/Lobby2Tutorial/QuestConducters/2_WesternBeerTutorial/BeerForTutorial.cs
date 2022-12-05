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

    private SyncOVRGrabbable _syncOVRGrabbable;

    private void Awake()
    {
        _initBeerPosition = transform.position;
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<BoxCollider>();

        _syncOVRGrabbable = GetComponent<SyncOVRGrabbable>();

        _syncOVRGrabbable.CallbackOnGrabBegin.RemoveListener(OnGrabBegin);
        _syncOVRGrabbable.CallbackOnGrabEnd.RemoveListener(OnGrabEnd);
        _syncOVRGrabbable.CallbackOnGrabBegin.AddListener(OnGrabBegin);
        _syncOVRGrabbable.CallbackOnGrabEnd.AddListener(OnGrabEnd);
    }

    private void OnEnable()
    {
        _myCollider.enabled = true;
        _myCollider.isTrigger = false;
        _beerCollider.isTrigger = false;

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

    private void OnGrabBegin()
    {
        Debug.Log("[Beer] ±×·¦ ½ÃÀÛµÊ");
        _myCollider.isTrigger = true;
        _beerCollider.isTrigger = true;
    }

    private void OnGrabEnd()
    {
        Debug.Log("[Beer] ±×·¦ ³¡³²");
        _myCollider.isTrigger = false;
        _beerCollider.isTrigger = false;
    }
}

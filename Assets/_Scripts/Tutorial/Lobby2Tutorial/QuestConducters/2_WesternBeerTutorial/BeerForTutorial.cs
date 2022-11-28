using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerForTutorial : MonoBehaviour
{
    [SerializeField] private GameObject _fullBeer;
    [SerializeField] private AudioClip _drinkSound;
    [SerializeField] private BoxCollider _grabCollider;
    [SerializeField] private BoxCollider _beerCollider;

    private Vector3 _initBeerPosition;
    private AudioSource _audioSource;

    private void Awake()
    {
        _initBeerPosition = transform.position;
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        transform.position = _initBeerPosition;
        _fullBeer.SetActive(true);
        _grabCollider.enabled = true;
    }

    public void DrinkBeer()
    {
        _audioSource.PlayOneShot(_drinkSound);
        _fullBeer.SetActive(false);
        _grabCollider.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private AudioSource _audioSource;
    private float _height = 5;
    private float _elapsedTime;
    [SerializeField]
    private AudioClip[] _clips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void GetCoin()
    {
        int _randNum = Random.Range(1,1000);
        if(_randNum == 1000)
        {
            _audioSource.PlayOneShot(_clips[2]);
        }
        else if(_randNum >=969)
        {
            _audioSource.PlayOneShot(_clips[1]);
        }
        else
        {
            _audioSource.PlayOneShot(_clips[0]);
        }
    }
    public void GetCoinEffect()
    {
        transform.position = 
            new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, transform.position.y + _height, 0));
    }    
}

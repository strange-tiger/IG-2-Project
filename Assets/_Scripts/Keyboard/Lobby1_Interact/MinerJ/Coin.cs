using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _clips;

    public void GetCoin()
    {
        int _randNum = Random.Range(1,1000);
        if(_randNum == 1000)
        {
            _audioSource.PlayOneShot(_clips[2]);
        }
        else if(_randNum <=969)
        {
            _audioSource.PlayOneShot(_clips[1]);
        }
        else
        {
            _audioSource.PlayOneShot(_clips[0]);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Coin : MonoBehaviour
{
    private AudioSource _audioSource;
    private Image _coinImage;

    [SerializeField]
    private Button _miningButton;
    [SerializeField]
    private AudioClip[] _clips;

    private float _delay = 5f;

    private float _elapsedTime = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _coinImage = gameObject.GetComponentInChildren<Image>();
    }

    public void GetCoin()
    {
        InitCoin();
        RandomGetCoin();
        StartCoroutine(GetCoinEffect());
    }

    public void RandomGetCoin()
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
    public void InitCoin()
    {
        gameObject.SetActive(true);
        _elapsedTime = 0;
        _coinImage.color = Color.white;
    }
    IEnumerator GetCoinEffect()
    {
        while(true)
        {
            _elapsedTime += Time.deltaTime;

            if(_elapsedTime > _delay)
            {
                gameObject.SetActive(false);
                _miningButton.gameObject.SetActive(true);
                break;
            }
            else
            {
                _coinImage.color = new Color( 1, 1, 1, 1f - _elapsedTime / _delay);
            }
            yield return null;
        }
    }    
}

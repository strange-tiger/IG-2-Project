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
    [SerializeField]
    private float _delay = 3;

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
            Debug.Log("100°ñµå");
            _audioSource.PlayOneShot(_clips[2]);
        }
        else if(_randNum >=969)
        {
            Debug.Log("10°ñµå");
            _audioSource.PlayOneShot(_clips[1]);
        }
        else
        {
            Debug.Log("1°ñµå");
            _audioSource.PlayOneShot(_clips[0]);
        }
    }
    public void InitCoin()
    {
        gameObject.SetActive(true);
        Color color = _coinImage.color;
        color.a = 1;
        _coinImage.color = color;
    }
    IEnumerator GetCoinEffect()
    {
        Debug.Log("ÄÚÀÎ È¿°ú");
        float _elapsedTime = 0;

        while(true)
        {
            _elapsedTime += Time.deltaTime;

            if(_elapsedTime > _delay)
            {
                Debug.Log($"{_delay}ÃÊ Áö³²");
                _coinImage.color = new Color(1, 1, 1, 0);
                gameObject.SetActive(false);
                _miningButton.gameObject.SetActive(true);
                break;
            }
            else
            {
                _coinImage.color = new Color(1, 1, 1, 1f - _elapsedTime / _delay);
            }
            yield return null;
        }
    }    
}

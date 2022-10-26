using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningSlider : MonoBehaviour
{
    private Slider _slider;
    private HitPoint _hitPoint;

    [SerializeField]
    private Coin _coin;

    private float _elapsedTime;
    public float ElapsedTime { get { return _elapsedTime; } set { _elapsedTime = value; } }

    [SerializeField]
    private float _delay = 10;

    private void Awake()
    {
        _slider = gameObject.GetComponent<Slider>();
        _hitPoint = transform.parent.GetComponentInChildren<HitPoint>();
    }
    private void OnEnable()
    {
        SliderInit();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if(_slider.value >= 1 )
        {
            SliderInit();
            GetChildTrans();
            _coin.GetCoin();
        }
        else
        {
            _slider.value = Mathf.Lerp(0, 1, _elapsedTime /_delay);
        }
    }

    private void SliderInit()
    {
        _slider.value = 0;
        _elapsedTime = 0;
    }
    public void GetChildTrans()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(false);
        }
    }
}

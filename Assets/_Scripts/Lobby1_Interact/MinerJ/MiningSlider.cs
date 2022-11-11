using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningSlider : MonoBehaviour
{
    private Slider _slider;
    [SerializeField]
    private Coin _coin;
    [SerializeField]
    private GameObject _circleCheckBox;

    private float _elapsedTime;
    public float ElapsedTime { get { return _elapsedTime; } set { _elapsedTime = value; } }
    private bool _isHitCircleEnable;
    public bool IsHitCircleEnable { get { return _isHitCircleEnable; } set { _isHitCircleEnable = value; } }

    [SerializeField]
    private float _delay = 10;

    private void Awake()
    {
        _slider = gameObject.GetComponent<Slider>();
    }
    private void OnEnable()
    {
        _isHitCircleEnable = false;
        SliderInit();
    }
    private void Update()
    {
        if(IsHitCircleEnable)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        if(_slider.value >= 1 )
        {
            gameObject.SetActive(false);
            HitCircleDisable();
            _coin.gameObject.SetActive(true);
        }
        else if (5f <= _elapsedTime && _elapsedTime <= 5.5f)
        {
            HitCircleEnable();
            IsHitCircleEnable = true;
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
    public void HitCircleDisable()
    {
        _circleCheckBox.gameObject.SetActive(false);
    }
    public void HitCircleEnable()
    {
        _circleCheckBox.gameObject.SetActive(true);
    }
}

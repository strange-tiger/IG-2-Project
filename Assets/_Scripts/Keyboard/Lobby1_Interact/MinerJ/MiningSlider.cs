using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningSlider : MonoBehaviour
{
    private Slider _slider;
    private HitPoint _hitPoint;
    private float _elapsedTime;

    [SerializeField]
    private float _delay = 10;

    private void Awake()
    {
        _slider = gameObject.GetComponent<Slider>();
        _hitPoint = transform.parent.GetComponentInChildren<HitPoint>();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        _slider.value = Mathf.Lerp(0, 1, _elapsedTime /_delay);

        if(_slider.value >= 1 )
        {
            _slider.value = 0;
            _elapsedTime = 0;
            GetChildTrans();

        }
    }
    public void GetChildTrans()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(false);
        }
    }
}

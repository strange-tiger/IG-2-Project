using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPoint : MonoBehaviour
{
    [SerializeField]
    private float _delay = 5.0f;
    [SerializeField]
    private CheckPoint _checkPoint;
    [SerializeField]
    private MiningSlider _miningSlider;
    [SerializeField]
    private Button _button;

    private Image _checkPointImage;
    private float _elapsedTime;
    private int _currentDegree = 0;

    private void Awake()
    {
        _checkPointImage = _checkPoint.gameObject.GetComponent<Image>();
    }
    private void OnEnable()
    {
        HitPointInit();
    }
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentDegree = (int)Mathf.Lerp(0f, 360f, _elapsedTime / _delay);
            if (_checkPoint.Angle >= _currentDegree && _currentDegree >= _checkPoint.Angle - (_checkPointImage.fillAmount * 360))
            {
                _miningSlider.ElapsedTime += 1f;
            }
            HitPointInit();
        }
        else if (_elapsedTime > _delay)
        {
            _miningSlider.GetChildTrans();
            _button.gameObject.SetActive(true);
        }
        else
        transform.rotation = Quaternion.Euler(0, 180, Mathf.Lerp(0f, 360f, _elapsedTime / _delay));
    }

    public void HitPointInit()
    {
        transform.rotation = Quaternion.Euler(0, 180, Mathf.Lerp(0f, 360f, 1f));
        _currentDegree = 0;
        _elapsedTime = 0f;
        _checkPoint.RandNum();
    }
}

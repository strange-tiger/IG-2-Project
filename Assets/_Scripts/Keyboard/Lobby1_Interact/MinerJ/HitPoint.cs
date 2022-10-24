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
    private Slider _miningSlider;

    private Image _checkPointImage;
    private float _elapsedTime;
    private int _currentDegree = 0;

    private void Awake()
    {
        _checkPointImage = _checkPoint.gameObject.GetComponent<Image>();
    }
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentDegree = (int)Mathf.Lerp(0f, 360f, _elapsedTime / _delay);
            if (_checkPoint.Angle >= _currentDegree && _currentDegree >= _checkPoint.Angle - (_checkPointImage.fillAmount * 360))
            {
                // 슬라이더 변환시킬거임!
                Debug.Log("슬라이더 변환!");
            }
            ResetPoint();
        }
        else if (_elapsedTime > _delay)
        {
            ResetPoint();
        }
        else
        transform.rotation = Quaternion.Euler(0, 180, Mathf.Lerp(0f, 360f, _elapsedTime / _delay));
    }

    public void ResetPoint()
    {
        transform.rotation = Quaternion.Euler(0, 180, Mathf.Lerp(0f, 360f, 1f));
        _currentDegree = 0;
        _elapsedTime = 0f;
        _checkPoint.RandNum();
    }
}

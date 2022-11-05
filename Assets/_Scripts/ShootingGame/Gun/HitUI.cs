using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitUI : MonoBehaviour
{
    [SerializeField] private float _hitUIDisableTime = 0.5f;
    private WaitForSeconds _waitForDisable;

    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        _waitForDisable = new WaitForSeconds(_hitUIDisableTime);
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CoDisableSelf());
    }

    private IEnumerator CoDisableSelf()
    {
        yield return _waitForDisable;
        gameObject.SetActive(false);
    }

    public void SetPointText(Color playerColor, int point)
    {
        _scoreText.color = playerColor;
        _scoreText.text = $"+{point}";
    }
}

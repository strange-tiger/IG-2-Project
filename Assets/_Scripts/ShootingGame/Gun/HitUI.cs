using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class HitUI : MonoBehaviourPun
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

    [PunRPC]
    public void SetPointText(Vector3 playerColor, int point, bool active)
    {
        _scoreText.gameObject.SetActive(active);
        if(active)
        {
            _scoreText.color = new Color(playerColor.x, playerColor.y, playerColor.z);
            _scoreText.text = point > 0 ? $"+{point}" : $"-{point}";
        }
    }
}

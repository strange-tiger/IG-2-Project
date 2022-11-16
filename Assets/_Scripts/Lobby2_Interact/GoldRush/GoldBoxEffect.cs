using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CoinGrade = Defines.ECoinGrade;

public class GoldBoxEffect : MonoBehaviour
{
    [SerializeField] private float _effectEndTime = 3f;
    private WaitForSeconds _waitForEffectEnd;

    [SerializeField] private TextMeshProUGUI _giveGoldText;

    [SerializeField]
    private AudioClip[] _goldCoinAudioClips =
        new AudioClip[(int)CoinGrade.Max];

    private GoldBoxSpawner _spawner;
    private GoldBoxSencer _sencer;
    private AudioSource _audioSource;

    private void Awake()
    {
        _sencer = transform.parent.GetComponent<GoldBoxSencer>();

        _audioSource = GetComponent<AudioSource>();

        _waitForEffectEnd = new WaitForSeconds(_effectEndTime);
    }

    private void OnEnable()
    {
        StartCoroutine(CoEndEffect());
    }

    public void SetEffect(int giveGold, int coinGrade, GoldBoxSpawner spawner)
    {
        _spawner = spawner;
        _audioSource.PlayOneShot(_goldCoinAudioClips[coinGrade]);
        _giveGoldText.text = $"+{giveGold}";
    }

    private IEnumerator CoEndEffect()
    {
        yield return _waitForEffectEnd;
        _spawner.ReturnToPoll(gameObject.transform.parent.gameObject);
        _sencer.enabled = true;
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}

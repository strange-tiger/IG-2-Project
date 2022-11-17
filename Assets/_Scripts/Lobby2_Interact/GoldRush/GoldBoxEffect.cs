using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using CoinGrade = Defines.ECoinGrade;

public class GoldBoxEffect : GoldBoxState
{
    [SerializeField] private float _effectEndTime = 3f;
    private WaitForSeconds _waitForEffectEnd;

    [SerializeField] private TextMeshProUGUI _giveGoldText;

    [SerializeField]
    private AudioClip[] _goldCoinAudioClips =
        new AudioClip[(int)CoinGrade.Max];
    [SerializeField] private AudioClip _fireWorkAudioClip;

    [SerializeField] private GameObject _canvas;
    [SerializeField] private GameObject _effect;

    private GoldBoxSpawner _spawner;
    private GoldBoxSencer _sencer;
    private AudioSource _audioSource;

    private int _coinGrade;

    private void Awake()
    {
        _sencer = transform.parent.GetComponent<GoldBoxSencer>();

        _audioSource = GetComponent<AudioSource>();

        _waitForEffectEnd = new WaitForSeconds(_effectEndTime);
    }

    public void SetEffect(int giveGold, int coinGrade, GoldBoxSpawner spawner)
    {
        _spawner = spawner;
        _coinGrade = coinGrade;
        _giveGoldText.text = $"+{giveGold}";
    }

    private void OnEnable()
    {
        _canvas.SetActive(true);
        photonView.RPC(nameof(ShowEffect), RpcTarget.All);
        StartCoroutine(CoEndEffect());
    }

    [PunRPC]
    private void ShowEffect()
    {
        _audioSource.PlayOneShot(_fireWorkAudioClip);
        _audioSource.PlayOneShot(_goldCoinAudioClips[_coinGrade]);
        _effect.SetActive(true);
    }

    private IEnumerator CoEndEffect()
    {
        yield return _waitForEffectEnd;
        _spawner.ReturnToPoll(gameObject.transform.parent.gameObject);
        photonView.RPC(nameof(ResetGoldBox), RpcTarget.All);
        //gameObject.SetActive(false);
        SetActiveObject(false);
    }
    
    [PunRPC]
    private void ResetGoldBox()
    {
        //_sencer.enabled = true;
        //transform.parent.gameObject.SetActive(false);
        _sencer.EnableScript(true);
        _sencer.SetActiveObject(false);

        _canvas.SetActive(false);
        _effect.SetActive(false);
    }
}

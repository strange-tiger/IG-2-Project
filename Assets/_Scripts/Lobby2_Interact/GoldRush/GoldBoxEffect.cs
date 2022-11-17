using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using CoinGrade = Defines.ECoinGrade;

public class GoldBoxEffect : MonoBehaviourPunCallbacks
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
        _canvas.SetActive(true);
    }

    public override void OnEnable()
    {
        base.OnEnable();

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
        photonView.RPC(nameof(ResetGoldBox), RpcTarget.All);
        //gameObject.SetActive(false);
        SetActiveObject(false);
    }
    
    [PunRPC]
    private void ResetGoldBox()
    {
        //_spawner.ReturnToPoll(gameObject.transform.parent.gameObject);
        transform.parent.parent = _spawner.GoldBoxParent;

        //_sencer.enabled = true;
        //transform.parent.gameObject.SetActive(false);
        _sencer.EnableScript(true);
        _sencer.SetActiveObject(false);

        _canvas.SetActive(false);
        _effect.SetActive(false);
    }

    public void EnableScript(bool value)
    {
        photonView.RPC(nameof(EnableScriptByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    private void EnableScriptByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Effect Scripts {value}");
        this.enabled = value;
    }

    public void SetActiveObject(bool value)
    {
        photonView.RPC(nameof(SetActiveObjectByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    private void SetActiveObjectByRPC(bool value)
    {
        Debug.Log($"[GoldRush] Effect Obejct {value}");
        gameObject.SetActive(value);
    }
}

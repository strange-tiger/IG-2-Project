using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

public class PlayerDebuffManager : MonoBehaviourPun
{
    [Header("Drunken Debuff")]
    [SerializeField] private GameObject _drunkenEffect;
    [SerializeField] private AudioClip _maleDrunkenSound;
    [SerializeField] private AudioClip _femaleDrunkenSound;

    [Header("Stun Debuff")]
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private AudioClip _stunSound;

    public Material FadeMaterial;

    private YieldInstruction _fadeTime = new WaitForSeconds(0.0001f);
    private YieldInstruction _drunkenTime = new WaitForSeconds(5f);
    private YieldInstruction _InvicibleTime = new WaitForSeconds(20f);

    private Color _initUIColor = new Color(1f, 1f, 0.28f, 0f);
    
    private AudioSource _audioSource;
    private PlayerCustomize _playerCustomize;
    private MeshRenderer _fadeRenderer;
    private PhotonVoiceView _playerVoiceView;

    private float _stunFadeTime = 2f;
    private float _drunkenFadeTime = 3f;
    private float _animatedFadeAlpha;

    private void OnEnable()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _playerCustomize = GetComponentInChildren<PlayerCustomize>();
        _playerVoiceView = GetComponent<PhotonVoiceView>();
    }

    private void Start()
    {
        StartCoroutine(CoFindFadeImage());
    }

    private IEnumerator CoFindFadeImage()
    {
        yield return new WaitForSeconds(4f);

        _fadeRenderer = GameObject.Find("CenterEyeAnchor").GetComponent<MeshRenderer>();
        _fadeRenderer.material = FadeMaterial;
        FadeMaterial.color = _initUIColor;
        _fadeRenderer.enabled = true;
    }

    private IEnumerator CoFade(float startAlpha, float endAlpha, float fadeTime)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            _animatedFadeAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));

            FadeMaterial.color = new Color(0, 0, 0, _animatedFadeAlpha);

            yield return _fadeTime;
        }

        _animatedFadeAlpha = endAlpha;
    }

    public void CallDrunkenDebuff()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("DrunkenDebuff", RpcTarget.All);
        }
    }


    [PunRPC]
    public void DrunkenDebuff()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(CoDrunkenFade());
        }
    }

    private IEnumerator CoDrunkenFade()
    {
        _playerVoiceView.enabled = false;

        _drunkenEffect.SetActive(true);

        if (_playerCustomize.IsFemale)
        {
            _audioSource.PlayOneShot(_femaleDrunkenSound);
        }
        else
        {
            _audioSource.PlayOneShot(_maleDrunkenSound);
        }

        StartCoroutine(CoFade(0, 1, _drunkenFadeTime));

        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;

        yield return _drunkenTime;

        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        StartCoroutine(CoFade(1, 0, _drunkenFadeTime));

        _playerVoiceView.enabled = true;

        _drunkenEffect.SetActive(false);

        yield return _InvicibleTime;

        PlayerControlManager.Instance.IsInvincible = false;
    }

    public void CallStunDebuff()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("StunDebuff", RpcTarget.All);
        }
    }

    [PunRPC]
    public void StunDebuff()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(StunFade());
        }
    }

    private IEnumerator StunFade()
    {
        _playerVoiceView.enabled = false;

        _stunEffect.SetActive(true);

        FadeMaterial.color = Color.black;

        _audioSource.PlayOneShot(_stunSound);

        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;

        StartCoroutine(CoFade(1, 0, _stunFadeTime));

        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        _stunEffect.SetActive(false);

        yield return _InvicibleTime;

        PlayerControlManager.Instance.IsInvincible = false;
    }
}

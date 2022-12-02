using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.PUN;

/*
 * 만취 상태와 기절 상태의 이펙트와 사운드, 화면 페이드 인, 아웃을 구현.
 */
public class PlayerDebuffManager : MonoBehaviourPun
{
    // 플레이어 만취 상태에 관련된 이펙트와 사운드.
    [Header("Drunken Debuff")]
    [SerializeField] private GameObject _drunkenEffect;
    [SerializeField] private AudioClip _maleDrunkenSound;
    [SerializeField] private AudioClip _femaleDrunkenSound;

    // 플레이어 기절 상태에 관련된 이펙트와 사운드.
    [Header("Stun Debuff")]
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private AudioClip _stunSound;

    // CenterEyeAnchor에 적용하여 페이드인, 아웃 시켜줌.
    public Material FadeMaterial;
    private Color _initUIColor = new Color(1f, 1f, 0.28f, 0f);

    // 각 코루틴에 사용되는 시간 변수
    private YieldInstruction _fadeTime = new WaitForSeconds(0.0001f);
    private YieldInstruction _drunkenTime = new WaitForSeconds(5f);
    private YieldInstruction _InvicibleTime = new WaitForSeconds(20f);

    
    // 플레이어에 관련된 컴포넌트
    private AudioSource _audioSource;
    private PlayerCustomize _playerCustomize;
    private MeshRenderer _fadeRenderer;
    private PhotonVoiceView _playerVoiceView;

    // 페이드 인, 아웃에 적용되는 시간값과 알파값 변수
    private float _stunFadeTime = 2f;
    private float _drunkenFadeTime = 3f;
    private float _animatedFadeAlpha;

    private void OnEnable()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
        _playerCustomize = GetComponentInChildren<PlayerCustomize>();
        _playerVoiceView = GetComponent<PhotonVoiceView>();
    }

    /// <summary>
    /// 플레이어가 생성된 후 OVRCamera에 있는 CenterEyeAnchor의 MeshRenderer를 찾음.
    /// </summary>
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

    /// <summary>
    /// 페이드 인, 페이드 아웃 시키는 코루틴
    /// </summary>
    /// <param name="startAlpha"> 시작 알파 값 </param>
    /// <param name="endAlpha"> 끝 알파 값 </param>
    /// <param name="fadeTime">페이드 인, 아웃 시킬 시간</param>
    /// <returns></returns>
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

    /// <summary>
    /// BeerInteraction에서 만취 상태 RPC를 호출함.
    /// </summary>
    public void CallDrunkenDebuff()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("DrunkenDebuff", RpcTarget.All);
        }
    }

    /// <summary>
    /// 만취 상태의 디버프를 적용시키는 코루틴을 호출하는 RPC 메서드.
    /// </summary>
    [PunRPC]
    public void DrunkenDebuff()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(CoDrunkenFade());
        }
    }

    /// <summary>
    /// 만취 상태의 디버프를 활성화 해주는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoDrunkenFade()
    {
        // 만취 상태가 시작되면 보이스 채팅을 비활성화.
        _playerVoiceView.enabled = false;

        // 만취 상태의 이펙트를 활성화.
        _drunkenEffect.SetActive(true);

        // 성별에 따라 다른 사운드를 출력시킴.
        if (_playerCustomize.IsFemale)
        {
            _audioSource.PlayOneShot(_femaleDrunkenSound);
        }
        else
        {
            _audioSource.PlayOneShot(_maleDrunkenSound);
        }

        // 페이드 아웃 코루틴 시작.
        StartCoroutine(CoFade(0, 1, _drunkenFadeTime));

        // 만취 상태에서는 움직이거나 Ray를 활성화 할 수 없고, 다른 기절이나 만취에 무적 상태가 됨.
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;

        yield return _drunkenTime;

        // 시간이 지나면 움직임과 Ray를 활성화.
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        // 화면을 페이드인.
        StartCoroutine(CoFade(1, 0, _drunkenFadeTime));

        // 보이스 채팅 활성화.
        _playerVoiceView.enabled = true;

        // 만취 이펙트 비활성화.
        _drunkenEffect.SetActive(false);

        yield return _InvicibleTime;

        // 만취가 풀린 후 20초가 지나면 무적 상태 해제.
        PlayerControlManager.Instance.IsInvincible = false;
    }

    /// <summary>
    /// 선빵필승, 오크통 컨텐츠에서 기절 상태 RPC를 호출하는 메서드.
    /// </summary>
    public void CallStunDebuff()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("StunDebuff", RpcTarget.All);
        }
    }

    /// <summary>
    /// 기절 상태의 디버프를 적용시키는 코루틴을 시작하는 RPC 메서드.
    /// </summary>
    [PunRPC]
    public void StunDebuff()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(StunFade());
        }
    }

    /// <summary>
    /// 기절 상태 디버프를 활성화 해주는 코루틴.
    /// </summary>
    /// <returns></returns>
    private IEnumerator StunFade()
    {
        // 보이스 채팅 비활성화.
        _playerVoiceView.enabled = false;

        // 기절 이펙트 활성화.
        _stunEffect.SetActive(true);

        // 화면을 검게 만들어줌.
        FadeMaterial.color = Color.black;

        // 기절 사운드를 출력한 후
        _audioSource.PlayOneShot(_stunSound);

        // 만취 디버프와 마찬가지로 무적 상태로 만들고, 움직임, Ray를 비활성화.
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;

        // 페이드 인 코루틴을 시작하고
        StartCoroutine(CoFade(1, 0, _stunFadeTime));

        // 끝났을 때, 다시 움직임과 Ray를 활성화.
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;

        // 기절 이펙트를 비활성화 하고
        _stunEffect.SetActive(false);

        yield return _InvicibleTime;

        // 20초가 지난 후 무적상태를 해제함.
        PlayerControlManager.Instance.IsInvincible = false;
    }
}

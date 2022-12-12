using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    /// <summary>
    /// 피격시 출력되는 효과
    /// </summary>
    [Header("2D AudioSource")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _stunClip;
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private float _stunEffectTime = 2.0f;

    /// <summary>
    /// 출력효과는 Local, Clone모두 실행, 스턴효과는 Local만 실행
    /// </summary>
    [PunRPC]
    public void OnDamageByBottle()
    {
        if (PlayerControlManager.Instance.IsInvincible == true)
        {
            return;
        }
        _stunEffect.SetActive(true);
        StartCoroutine(CoStunEffectOver());

        if (photonView.IsMine == false)
        {
            return;
        }

        GetComponentInChildren<OVRScreenFade>()?.FadeOut(0.0f);
        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;
        _audioSource.PlayOneShot(_stunClip);
        StartCoroutine(CoInvincible());
        StartCoroutine(CoReviveCooldown());
    }

    YieldInstruction _invincibleTime = new WaitForSeconds(20f);
    private IEnumerator CoInvincible()
    {
        yield return _invincibleTime;
        PlayerControlManager.Instance.IsInvincible = false;
    }

    YieldInstruction _reviveCooldown = new WaitForSeconds(2.0f);
    private IEnumerator CoReviveCooldown()
    {
        yield return _reviveCooldown;
        Revive();
    }
    private void Revive()
    {
        GetComponentInChildren<OVRScreenFade>()?.FadeIn(2.0f);
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
    }

    private IEnumerator CoStunEffectOver()
    {
        yield return new WaitForSeconds(_stunEffectTime);
        _stunEffect.SetActive(false);
    }
}

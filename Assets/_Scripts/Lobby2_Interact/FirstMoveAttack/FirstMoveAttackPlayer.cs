using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class FirstMoveAttackPlayer : MonoBehaviourPun
{
    [Header("2D AudioSource")]
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _stunClip;
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private float _stunEffectTime = 2.0f;

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
        StartCoroutine(ReviveCooldown());
    }

    public void Revive()
    {
        GetComponentInChildren<OVRScreenFade>()?.FadeIn(2.0f);
        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
    }

    YieldInstruction _reviveCooldown = new WaitForSeconds(2.0f);
    IEnumerator ReviveCooldown()
    {
        yield return _reviveCooldown;
        Revive();
    }

    YieldInstruction _invincibleTime = new WaitForSeconds(20f);
    private IEnumerator CoInvincible()
    {
        yield return _invincibleTime;
        PlayerControlManager.Instance.IsInvincible = false;
    }

    private IEnumerator CoStunEffectOver()
    {
        yield return new WaitForSeconds(_stunEffectTime);
        _stunEffect.SetActive(false);
    }
}

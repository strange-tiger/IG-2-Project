using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstMoveAttackNPCForTurorial : MonoBehaviour
{
    public delegate void OnHit();
    public event OnHit OnNPCHit;

    private AudioSource _audioSource;
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private float _stunEffectTime = 2.0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void onDamageByBottle()
    {
        Debug.Log("[Tutorial: FirstHit] NPC 피격 당함");

        // 스턴 효과 표시
        _audioSource.Play();
        _stunEffect.SetActive(true);
        StartCoroutine(CoStunEffectOver());

        OnNPCHit.Invoke();
    }

    private IEnumerator CoStunEffectOver()
    {
        yield return new WaitForSeconds(_stunEffectTime);
        _stunEffect.SetActive(false);
    }
}

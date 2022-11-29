using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestEnd = QuestConducter.QuestEnd;

public class OakBarrelNPCForTurorial : InteracterableObject
{
    public event QuestEnd OnQuestEnd;
    [SerializeField] private GameObject _oakBarrel;

    private AudioSource _audioSource;
    [SerializeField] private GameObject _stunEffect;
    [SerializeField] private float _stunEffectTime = 2.0f;

    protected override void Awake()
    {
        base.Awake();

        _audioSource = GetComponent<AudioSource>();
    }

    public void PrepareForQuest()
    {
        StopAllCoroutines();
        _stunEffect.SetActive(false);
        _oakBarrel.SetActive(true);
    }

    public override void Interact()
    {
        _audioSource.Play();
        _stunEffect.SetActive(true);
        StartCoroutine(CoStunEffectOver());
        _oakBarrel.SetActive(false);

        OnQuestEnd.Invoke();
    }

    private IEnumerator CoStunEffectOver()
    {
        yield return new WaitForSeconds(_stunEffectTime);
        _stunEffect.SetActive(false);
    }
}

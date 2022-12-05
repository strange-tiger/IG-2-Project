using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestConducter : MonoBehaviour
{
    public delegate void QuestEnd();
    public event QuestEnd OnQuestEnd;

    private AudioClip _questStartSound;
    private AudioClip _questEndSound;
    private AudioSource _audioSource;

    private bool _isAudioSet = false;

    public void SetQuestSound(AudioClip _startSound, AudioClip _endSound)
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<EffectPlayer>();

        _questStartSound = _startSound;
        _questEndSound = _endSound;
        _isAudioSet = true;
    }

    protected virtual void OnEnable()
    {
        OnQuestEnd -= OnQuestEnded;
        OnQuestEnd += OnQuestEnded;
        StartQuest();
    }

    public virtual void StartQuest()
    {
        if(_isAudioSet)
        {
            _audioSource.PlayOneShot(_questStartSound);
        }
    }

    protected virtual void OnQuestEnded()
    {
        _audioSource.PlayOneShot(_questEndSound);
    }

    protected void QuestEnded()
    {
        OnQuestEnd.Invoke();
    }
}

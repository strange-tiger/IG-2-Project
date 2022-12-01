using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    private AudioSource _effectPlayer;
    private void Awake()
    {
        _effectPlayer = GetComponent<AudioSource>();
        SoundManager.Instance.OnChangedEffectVolume.RemoveListener(UpdateVolume);
        SoundManager.Instance.OnChangedEffectVolume.AddListener(UpdateVolume);
    }

    public void UpdateVolume(float effectVolume)
    {
        _effectPlayer.volume = effectVolume;
    }

    private void OnDestroy()
    {
        SoundManager.Instance.OnChangedEffectVolume.RemoveListener(UpdateVolume);
    }
}

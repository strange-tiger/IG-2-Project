using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _effectPlayer;
    private void Awake()
    {
        SoundManager.Instance.OnChangedEffectVolume = UpdateVolume;
    }

    private void OnEnable()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        _effectPlayer.volume =
            SoundManager.Instance.MasterVolume * SoundManager.Instance.EffectVolume;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioSource _bgmPlayer;
    [SerializeField]
    private AudioClip _bgmClip;
    private void Awake()
    {
        _bgmPlayer = gameObject.AddComponent<AudioSource>();     
        _bgmPlayer.clip = _bgmClip;
        _bgmPlayer.loop = true;
        _bgmPlayer.playOnAwake = true;

        SoundManager.Instance.OnChangedBackgroundVolume = UpdateVolume;
    }

    public void UpdateVolume()
    {
        _bgmPlayer.volume =
            SoundManager.Instance.MasterVolume * SoundManager.Instance.BackgroundVolume;
    }
}

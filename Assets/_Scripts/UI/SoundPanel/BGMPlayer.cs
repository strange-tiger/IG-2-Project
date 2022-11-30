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
        SoundManager.Instance.OnChangedBackgroundVolume = UpdateVolume;
        
        _bgmPlayer = gameObject.AddComponent<AudioSource>();     
        _bgmPlayer.clip = _bgmClip;
        _bgmPlayer.loop = true;
        _bgmPlayer.Play();
    }

    public void UpdateVolume()
    {
        _bgmPlayer.volume =
            SoundManager.Instance.MasterVolume * SoundManager.Instance.BackgroundVolume;
    }
}

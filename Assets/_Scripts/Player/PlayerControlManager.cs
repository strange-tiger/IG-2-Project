using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager: GlobalInstance<PlayerControlManager>
{
    public bool IsMoveable { get; set; } = true;
    public bool IsRayable { get; set; } = true;
    [SerializeField] private bool _isInvincible = false;
    public bool IsInvincible { get => _isInvincible; set => _isInvincible = value; }

    [SerializeField] 
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip[] _audioClipList;
    [SerializeField] 
    private ParticleSystem _stundParticle;

    const int STUN_SOUND = 0;
    const int BEER_SOUND = 1;
    public void SetStun(bool stun)
    {
        if(stun)
        {
            _audioSource.PlayOneShot(_audioClipList[STUN_SOUND]);
            _stundParticle.Play();
        }
    }
}

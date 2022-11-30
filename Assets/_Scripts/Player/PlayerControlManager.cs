using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager: GlobalInstance<PlayerControlManager>
{
    public bool IsMoveable { get; set; } = true;
    public bool IsRayable { get; set; } = true;
    [SerializeField] private bool _isInvincible = false;
    public bool IsInvincible { get => _isInvincible; set => _isInvincible = value; }

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ParticleSystem _stundParticle;
    public bool IsStund
    {
         set
        {
            if(value)
            {
                // 스턴 효과 실행
                IsMoveable = false;
                IsRayable = false;
                _audioSource.Play();
                _stundParticle.Play();
            }
            else
            {
                IsMoveable = true;
                IsRayable = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicWandReUse : MonoBehaviourPun
{
    [SerializeField] private MagicWand _magicWand;

    private float _currentTime;

    private void Update()
    {
        if (photonView.IsMine)
        {
            // ÄðÅ¸ÀÓ
            if (_magicWand.CheckCoolTime)
            {
                _currentTime += Time.deltaTime;

                _magicWand.CoolTimeText = (int)_magicWand.CoolTime;

                _magicWand.CoolTimeText -= (int)_currentTime;

                if (_currentTime > (float)_magicWand.CoolTime)
                {
                    _currentTime -= _currentTime;
                    _magicWand.CheckCoolTime = false;
                }
            }
        }
    }
}

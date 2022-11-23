using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHandRigging : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Transform[] _modelHandTransforms;
    [SerializeField] protected Transform[] _ovrHandTransforms;
    private const int _HAND_COUNT = 2;

    protected virtual void FixedUpdate()
    {
        for(int i = 0; i < _HAND_COUNT; ++i)
        {
            _modelHandTransforms[i].position = _ovrHandTransforms[i].position;
            _modelHandTransforms[i].rotation = _ovrHandTransforms[i].rotation;
        }
    }
}

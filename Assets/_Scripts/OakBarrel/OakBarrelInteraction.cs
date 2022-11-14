using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _oakBarrelObject;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private OakBarrel _oakBarrel;

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(120f);
    private PlayerControllerMove _playerControllerMove;

    private float _speedSlower = 0.2f;
    private bool _isInOak;

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();

        _oakBarrel.CoveredOakBarrel.AddListener(BecomeOakBarrel);
    }

    private void Update()
    {
        if (_isInOak == true && OVRInput.GetDown(OVRInput.Button.One))
        {
            StopCoroutine(OakBarrelIsGone());

            OutOakBarrel();
        }
    }

    private void BecomeOakBarrel()
    {
        if (photonView.IsMine)
        {
            if (_playerModel.activeSelf == false)
            {
                InOakBarrel();

                StartCoroutine(OakBarrelIsGone());
            }

            if (_playerModel.activeSelf == true)
            {
                OutOakBarrel();
            }
        }
    }

    private IEnumerator OakBarrelIsGone()
    {
        yield return _oakBarrelReturnTime;

        OutOakBarrel();
    }

    [PunRPC]
    private void ActiveSetting(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }

    private void InOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, false);
        photonView.RPC("ActiveSetting", RpcTarget.All, _oakBarrelObject, true);

        _isInOak = true;

        _playerControllerMove.MoveScale -= _speedSlower;
    }

    private void OutOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, true);
        photonView.RPC("ActiveSetting", RpcTarget.All, _oakBarrelObject, false);

        _isInOak = false;

        _playerControllerMove.MoveScale += _speedSlower;
    }
}
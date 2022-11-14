using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _playerOakBarrel;
    [SerializeField] private GameObject _playerModel;
    private OakBarrel _oakBarrel;

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(5f);
    private PlayerControllerMove _playerControllerMove;

    private float _speedSlower = 0.2f;
    private bool _isInOak;

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();

        _oakBarrel = GameObject.Find("OakBarrel").GetComponent<OakBarrel>();

        _oakBarrel.CoveredOakBarrel.RemoveListener(BecomeOakBarrel);
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
        Debug.Log("1");
        if (photonView.IsMine)
        {
            if (_playerModel.activeSelf == true)
            {
                Debug.Log("2");

                InOakBarrel();

                StartCoroutine(OakBarrelIsGone());
            }

            if (_playerModel.activeSelf == false)
            {
                Debug.Log("3");
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
    public void ActiveSetting(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }

    private void InOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, false);
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerOakBarrel, true);

        _isInOak = true;

        _playerControllerMove.MoveScale -= _speedSlower;
    }

    private void OutOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, true);
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerOakBarrel, false);

        _isInOak = false;

        _playerControllerMove.MoveScale += _speedSlower;
    }
}
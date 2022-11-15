using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _playerOakBarrel;
    [SerializeField] private GameObject _playerModel;

    private PlayerInteraction _playerInteraction;

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(120f);
    private PlayerControllerMove _playerControllerMove;

    private float _speedSlower = 0.2f;
    private bool _isInOak;

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();
        _playerInteraction = GetComponentInChildren<PlayerInteraction>();

        _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
        _playerInteraction.InteractionOakBarrel.AddListener(BecomeOakBarrel);
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
            if (_playerModel.activeSelf == true)
            {
                InOakBarrel();

                StartCoroutine(OakBarrelIsGone());
            }

            else if (_playerModel.activeSelf == false)
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
    public void ActivePlayer(bool value)
    {
        Debug.Log($"ActivePlayer : {value}");

        _playerModel.SetActive(value);
    }

    [PunRPC]
    public void ActiveOakBarrel(bool value)
    {
        Debug.Log($"ActiveOakBarrel: {value}");

        _playerOakBarrel.SetActive(value);
    }

    private void InOakBarrel()
    {
        Debug.Log("InOakBarrel");

        photonView.RPC("ActiveOakBarrel", RpcTarget.All, true);
        photonView.RPC("ActivePlayer", RpcTarget.All, false);

        _isInOak = true;

        _playerControllerMove.MoveScale -= _speedSlower;
    }

    private void OutOakBarrel()
    {
        Debug.Log("OutOakBarrel");

        photonView.RPC("ActiveOakBarrel", RpcTarget.All, false);
        photonView.RPC("ActivePlayer", RpcTarget.All, true);

        _isInOak = false;

        _playerControllerMove.MoveScale += _speedSlower;
    }

    private void OnDisable()
    {
        _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
    }
}
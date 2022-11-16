using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _playerOakBarrel;
    [SerializeField] private GameObject _playerModel;

    private PlayerInteraction _playerInteraction;

    private static WaitForSeconds _oakBarrelReturnTime = new WaitForSeconds(7f);
    private PlayerControllerMove _playerControllerMove;

    private MeshRenderer _playerMeshRenderer;
    private MeshRenderer _oakBarrelMeshRenderer;

    private Color _color = new Color(0, 0, 0, 0);
    
    private float _speedSlower = 0.2f;
    private bool _isInOak;

    private void Start()
    {
        _playerControllerMove = GetComponent<PlayerControllerMove>();
        _playerInteraction = GetComponentInChildren<PlayerInteraction>();

        _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
        _playerInteraction.InteractionOakBarrel.AddListener(BecomeOakBarrel);

        _playerMeshRenderer = GameObject.Find("CenterEyeAnchor").GetComponentInChildren<MeshRenderer>();

        _oakBarrelMeshRenderer = GameObject.Find("Oakbarrel").GetComponent<MeshRenderer>();
        
    }

    private void Update()
    {
        if (_isInOak == true && OVRInput.GetDown(OVRInput.Button.One))
        {
            StopCoroutine(OakBarrelIsGone());

            OutOakBarrel();
        }

        if (_oakBarrelMeshRenderer.enabled == false && _playerModel.activeSelf == false)
        {
            _playerMeshRenderer.material.color = Color.black;
            StartCoroutine(FadeOutPlayerScreen());

            OutOakBarrel();
        }
    }

    private void BecomeOakBarrel()
    {
        if (photonView.IsMine)
        {
            InOakBarrel();
            StartCoroutine(OakBarrelIsGone());
        }
    }

    private IEnumerator OakBarrelIsGone()
    {
        yield return _oakBarrelReturnTime;

        OutOakBarrel();
    }

    private IEnumerator FadeOutPlayerScreen()
    {
        yield return new WaitForSeconds(2f);

        _playerMeshRenderer.material.color = _color;
    }


    [PunRPC]
    public void ActivePlayer(bool value)
    {
        _playerModel.SetActive(value);
    }

    [PunRPC]
    public void ActiveOakBarrel(bool value)
    {
        _oakBarrelMeshRenderer.enabled = value;
    }

    private void InOakBarrel()
    {
        photonView.RPC("ActiveOakBarrel", RpcTarget.All, true);
        photonView.RPC("ActivePlayer", RpcTarget.All, false);

        _isInOak = true;

        _playerControllerMove.MoveScale -= _speedSlower;

        PlayerControlManager.Instance.IsRayable = false;
    }

    private void OutOakBarrel()
    {
        photonView.RPC("ActiveOakBarrel", RpcTarget.All, false);
        photonView.RPC("ActivePlayer", RpcTarget.All, true);

        _isInOak = false;

        _playerControllerMove.MoveScale += _speedSlower;

        PlayerControlManager.Instance.IsRayable = true;
    }

    private void OnDisable()
    {
        _playerInteraction.InteractionOakBarrel.RemoveListener(BecomeOakBarrel);
    }
}
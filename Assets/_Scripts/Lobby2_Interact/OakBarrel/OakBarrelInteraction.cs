using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OakBarrelInteraction : MonoBehaviourPun
{
    [SerializeField] private GameObject _oakBarrelObject;
    [SerializeField] private GameObject _playerModel;
    [SerializeField] private Transform _modelTransform;
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
            // 내가 오크통이 아닐 때
            if (_playerModel.activeSelf == false)
            {
                InOakBarrel();

                StartCoroutine(OakBarrelIsGone());
            }

            // 내가 오크통일 때
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

    /// <summary>
    /// 오브젝트들 껏다 켯다 할 수 있게 해주는 RPC함수
    /// </summary>
    /// <param name="obj">오브젝트</param>
    /// <param name="value">키면 True 끄면 False</param>
    [PunRPC]
    private void ActiveSetting(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }

    /// <summary>
    /// 오크통으로 들어감
    /// </summary>
    private void InOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, false);
        photonView.RPC("ActiveSetting", RpcTarget.All, _oakBarrelObject, true);

        _isInOak = true;

        _playerControllerMove.MoveScale -= _speedSlower;
    }

    /// <summary>
    /// 오크통에서 나감
    /// </summary>
    private void OutOakBarrel()
    {
        photonView.RPC("ActiveSetting", RpcTarget.All, _playerModel, true);
        photonView.RPC("ActiveSetting", RpcTarget.All, _oakBarrelObject, false);

        _isInOak = false;

        _playerControllerMove.MoveScale += _speedSlower;
    }
}

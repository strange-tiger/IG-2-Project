//#define _Photon
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WoodPile : InteracterableObject
{
    [SerializeField] GameObject _wood;
    private static readonly YieldInstruction INTERACT_COOLTIME = new WaitForSeconds(5f);
    private bool _onCooltime = false;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
            stream.SendNext(_onCooltime);
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
            _onCooltime = true;
        }
    }

    public override void Interact()
    {
        base.Interact();

        if (_onCooltime)
        {
            return;
        }
#if _Photon
        photonView.RPC("SpawnWood", RpcTarget.All);
#else
        Instantiate(_wood, new Vector3(0f, 2f, 0f), Quaternion.Euler(0f, 0f, 0f));
        StartCoroutine(CalculateCooltime());
#endif
    }

    [PunRPC]
    private void SpawnWood()
    {
        PhotonNetwork.Instantiate("Wood", transform.position, transform.rotation);
        StartCoroutine(CalculateCooltime());
    }

    private IEnumerator CalculateCooltime()
    {
        _onCooltime = true;

        yield return INTERACT_COOLTIME;

        _onCooltime = false;
    }
}

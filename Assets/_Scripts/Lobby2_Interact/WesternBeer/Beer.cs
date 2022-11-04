using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

public class Beer : InteracterableObject, IPunObservable
{


    [SerializeField] GameObject _fullBeer;
    private MeshCollider _meshCollider;
    private Vector3 _initBeerPosition;
    private YieldInstruction _regenerateTime = new WaitForSeconds(30f);


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_fullBeer.activeSelf);
            stream.SendNext(_meshCollider.enabled);
        }
        else if (stream.IsReading)
        {
            _fullBeer.SetActive((bool)stream.ReceiveNext());
            _meshCollider.enabled = (bool)stream.ReceiveNext();
        }
    }



    public void Start()
    {
        _initBeerPosition = transform.localPosition;
        _meshCollider = GetComponent<MeshCollider>();
    }

    public void CallDrinkBeer()
    {
          photonView.RPC("DrinkBeer", RpcTarget.All);
    }

    [PunRPC]
    public void DrinkBeer()
    {
        _fullBeer.SetActive(false);
        _meshCollider.enabled = false;
        StartCoroutine(ReGenerateBeer());
    }



    private IEnumerator ReGenerateBeer()
    {
        yield return _regenerateTime;

        transform.position = _initBeerPosition;

        _fullBeer.SetActive(true);

        _meshCollider.enabled = false;

        yield return null;
    }





}

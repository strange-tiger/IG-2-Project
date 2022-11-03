using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerModelHandConnection : MonoBehaviourPun, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            transform.position = (Vector3) stream.ReceiveNext();
            Debug.Log( "[VR] " + transform.position);
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}

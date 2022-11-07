using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestTest : MonoBehaviourPun
{
    

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ぞし");

            if (OVRInput.GetDown(OVRInput.Button.Two))
            {
                Debug.Log("しぞ");
                PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
            }
        }
    }
}

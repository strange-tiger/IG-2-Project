using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : MonoBehaviourPun
{
    [SerializeField] private GameObject _ovrCameraRigPrefab;

    private void Awake()
    {
        if(photonView.IsMine)
        {
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();
        }
        else
        {
            gameObject.AddComponent<CapsuleCollider>();
        }
    }
}

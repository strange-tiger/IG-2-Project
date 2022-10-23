using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : MonoBehaviourPun
{
    [SerializeField] private Vector3 _ovrCameraPosition;
    [SerializeField] private GameObject _ovrCameraRigPrefab;

    private void Awake()
    {
        if(photonView.IsMine)
        {
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            cameraRig.transform.position = _ovrCameraPosition;

            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();
        }
    }
}

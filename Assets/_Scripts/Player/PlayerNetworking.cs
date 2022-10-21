using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworking : MonoBehaviourPun
{
    [SerializeField] private GameObject _ovrCameraRigPrefab;
    public string MyNickname { get; private set; }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();
        }
        else
        {
            gameObject.AddComponent<CapsuleCollider>().height = 2f;
            gameObject.AddComponent<OtherPlayerInteraction>();
        }
    }

    private void OnEnable()
    {
        
    }

    private void SetPlayerNickname(string nickname)
    {
        MyNickname = nickname;
    }
}

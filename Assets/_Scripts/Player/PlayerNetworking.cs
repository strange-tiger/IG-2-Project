using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNetworking : MonoBehaviourPun
{
    [SerializeField] private Vector3 _ovrCameraPosition;
    [SerializeField] private GameObject _ovrCameraRigPrefab;
    [SerializeField] private TextMeshProUGUI _nicknameText;

    public string MyNickname { get; private set; }
    public string MyUserId { get; private set; }

    private void Awake()
    {
        if(photonView.IsMine)
        {
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            cameraRig.transform.position = _ovrCameraPosition;

            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();
        }
        else
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.height = 2f;

            gameObject.AddComponent<UserInteraction>();
        }
    }

    [PunRPC]
    public void SetNickname(string id, string nickname)
    {
        MyNickname = nickname;
        _nicknameText.text = nickname;

        MyUserId = id;
    }
}

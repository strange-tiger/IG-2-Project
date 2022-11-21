using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingPlayerNetworking : BasicPlayerNetworking
{
    [SerializeField] private GameObject _requestAlarmImage;

    protected override void Awake()
    {
        base.Awake();
        if(photonView.IsMine)
        {
            // �÷��̾� ��Ʈ�ѷ� ����
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            NewPlayerMove newPlayerMove = gameObject.AddComponent<NewPlayerMove>();
            playercontroller.CameraRig = _myOVRCameraRig.GetComponent<OVRCameraRig>();

            // �Ҽ� �˸� ��� ����
            SocialTabManager socialTabManager = 
                _myOVRCameraRig.GetComponentInChildren<SocialTabManager>();
            socialTabManager.RequestAlarmImage = _requestAlarmImage;
            socialTabManager.gameObject.SetActive(false);

            socialTabManager.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.height = 2f;
        }
        gameObject.AddComponent<UserInteraction>().RequestAlarmImage = _requestAlarmImage;
    }
}

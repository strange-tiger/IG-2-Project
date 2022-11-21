using Asset.MySql;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : BasicPlayerNetworking
{
    [SerializeField] private GameObject _requestAlarmImage;

    [SerializeField] private AudioSource _newPlayerAudioSource;

    protected override void Awake()
    {
        if (photonView.IsMine)
        {
            // CameraRig ����
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            //GameObject cameraRig = GetComponentInChildren<OVRCameraRig>().gameObject;
            cameraRig.transform.localPosition = _ovrCameraPosition;

            // �÷��̾� ��Ʈ�ѷ� ����
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            NewPlayerMove newPlayerMove = gameObject.AddComponent<NewPlayerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();

            // �÷��̾� �� ����
            // ���� �޾ƿ� ������
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            _ovrCameraHandTransforms[0] = hands[0].transform.parent.GetChild(0);
            _ovrCameraHandTransforms[1] = hands[1].transform.parent.GetChild(0);

            //OVRGrabber[] grabber = cameraRig.GetComponentsInChildren<OVRGrabber>();
            //grabber[0].ParentTransform = _modelHandTransforms[0];
            //grabber[1].ParentTransform = _modelHandTransforms[1];


            // �Ҽ� �˸���� ����
            SocialTabManager socialTabManager = cameraRig.GetComponentInChildren<SocialTabManager>();
            socialTabManager.RequestAlarmImage = _requestAlarmImage;
            socialTabManager.gameObject.SetActive(false);

            // ���� ���� ��ũ��Ʈ ����
            VolumeController volumeController = cameraRig.GetComponentInChildren<VolumeController>();
            volumeController.PlayerAudioSource = _newPlayerAudioSource;

            // UI �ٽ� ��Ȱ��ȭ
            volumeController.transform.parent.gameObject.SetActive(false);

            socialTabManager.transform.parent.gameObject.SetActive(false);

            // ���� ���� canvas�� �����ϱ� ���� ������ ��������
            _pointer = cameraRig.GetComponentInChildren<OVRGazePointer>().gameObject;
        }
        else
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.height = 2f;

        }
        gameObject.AddComponent<UserInteraction>().RequestAlarmImage = _requestAlarmImage;
    }

  
}

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
            // CameraRig 부착
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            //GameObject cameraRig = GetComponentInChildren<OVRCameraRig>().gameObject;
            cameraRig.transform.localPosition = _ovrCameraPosition;

            // 플레이어 컨트롤러 부착
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            NewPlayerMove newPlayerMove = gameObject.AddComponent<NewPlayerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();

            // 플레이어 모델 연결
            // 손을 받아와 연결함
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            _ovrCameraHandTransforms[0] = hands[0].transform.parent.GetChild(0);
            _ovrCameraHandTransforms[1] = hands[1].transform.parent.GetChild(0);

            //OVRGrabber[] grabber = cameraRig.GetComponentsInChildren<OVRGrabber>();
            //grabber[0].ParentTransform = _modelHandTransforms[0];
            //grabber[1].ParentTransform = _modelHandTransforms[1];


            // 소셜 알림기능 연결
            SocialTabManager socialTabManager = cameraRig.GetComponentInChildren<SocialTabManager>();
            socialTabManager.RequestAlarmImage = _requestAlarmImage;
            socialTabManager.gameObject.SetActive(false);

            // 사운드 세팅 스크립트 연결
            VolumeController volumeController = cameraRig.GetComponentInChildren<VolumeController>();
            volumeController.PlayerAudioSource = _newPlayerAudioSource;

            // UI 다시 비활성화
            volumeController.transform.parent.gameObject.SetActive(false);

            socialTabManager.transform.parent.gameObject.SetActive(false);

            // 월드 내의 canvas와 연결하기 위한 포인터 가져오기
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

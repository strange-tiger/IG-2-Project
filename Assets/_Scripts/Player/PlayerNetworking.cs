using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Photon.Voice.Unity;

public class PlayerNetworking : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3 _ovrCameraPosition = new Vector3(0f, 0.7f, 0.8f);
    [SerializeField] private GameObject _ovrCameraRigPrefab;
    [SerializeField] private TextMeshProUGUI _nicknameText;
    [SerializeField] private GameObject _requestAlarmImage;
    [SerializeField] private AudioSource _newPlayerAudioSource;
    private Recorder _recorder;

    private GameObject _pointer;

    public string MyNickname { get; private set; }
    public string MyUserId { get; private set; }

    private void Awake()
    {
        if(photonView.IsMine)
        {
            // CameraRig 부착
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            cameraRig.transform.position = _ovrCameraPosition;

            // 플레이어 컨트롤러 부착
            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();

            // 플레이어 모델 연결
            GameObject modelGameObject = gameObject.GetComponentInChildren<Animator>().gameObject;
            PlayerModelAnimation playerModelAniamtion = modelGameObject.GetComponent<PlayerModelAnimation>();
            // 손을 받아와 연결함
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            playerModelAniamtion.LeftHand = hands[0].transform.parent.GetChild(0);
            playerModelAniamtion.RightHand = hands[1].transform.parent.GetChild(0);

            // 소셜 알림기능 연결
            SocialTabManager socialTabManager = cameraRig.GetComponentInChildren<SocialTabManager>();
            socialTabManager.RequestAlarmImage = _requestAlarmImage;
            socialTabManager.gameObject.SetActive(false);

            // 사운드 세팅 스크립트 연결
            VolumeController volumeController = cameraRig.GetComponentInChildren<VolumeController>();
            volumeController.PlayerAudioSource = _newPlayerAudioSource;

            _recorder = GetComponentInChildren<Recorder>();
            ScrollButton scrollButton = cameraRig.GetComponentInChildren<ScrollButton>();
            scrollButton.PhotonVoice = _recorder;

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

    [PunRPC]
    public void SetNickname(string id, string nickname)
    {
        MyNickname = nickname;
        _nicknameText.text = nickname;

        MyUserId = id;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("SetNickname", newPlayer, MyUserId, MyNickname);
    }

    public void CanvasSetting(OVRRaycaster[] ovrRaycasters)
    {
        foreach(OVRRaycaster canvas in ovrRaycasters)
        {
            canvas.pointer = _pointer;
        }
    }
}

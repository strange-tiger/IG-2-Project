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
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            cameraRig.transform.position = _ovrCameraPosition;

            PlayerControllerMove playercontroller = gameObject.AddComponent<PlayerControllerMove>();
            playercontroller.CameraRig = cameraRig.GetComponent<OVRCameraRig>();

            SocialTabManager socialTabManager = cameraRig.GetComponentInChildren<SocialTabManager>();
            socialTabManager.RequestAlarmImage = _requestAlarmImage;
            socialTabManager.gameObject.SetActive(false);

            VolumeController volumeController = cameraRig.GetComponentInChildren<VolumeController>();
            volumeController.PlayerAudioSource = _newPlayerAudioSource;

            _recorder = GetComponentInChildren<Recorder>();
            ScrollButton scrollButton = cameraRig.GetComponentInChildren<ScrollButton>();
            scrollButton.PhotonVoice = _recorder;

            volumeController.transform.parent.gameObject.SetActive(false);

            socialTabManager.transform.parent.gameObject.SetActive(false);

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

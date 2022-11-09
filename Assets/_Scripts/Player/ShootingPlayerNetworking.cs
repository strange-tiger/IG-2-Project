using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ShootingPlayerNetworking : MonoBehaviourPunCallbacks
{
    [SerializeField] private Vector3 _ovrCameraPosition = new Vector3(0f, 0.7f, 0.8f);
    [SerializeField] private GameObject _ovrCameraRigPrefab;

    private const int HAND_COUNT = 2;
    [SerializeField] private Transform[] _modelHandTransforms;
    private Transform[] _ovrCameraHandTransforms = new Transform[2];

    [SerializeField] private TextMeshProUGUI _nicknameText;

    [SerializeField] private AudioSource _newPlayerAudioSource;

    private GameObject _pointer;

    public string MyNickname { get; private set; }
    public string MyUserId { get; private set; }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            // CameraRig ����
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            cameraRig.transform.position = _ovrCameraPosition;

            // ���� �޾ƿ� ������
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            _ovrCameraHandTransforms[0] = hands[0].transform.parent.GetChild(0);
            _ovrCameraHandTransforms[1] = hands[1].transform.parent.GetChild(0);

            // ���� ���� canvas�� �����ϱ� ���� ������ ��������
            _pointer = cameraRig.GetComponentInChildren<OVRGazePointer>().gameObject;
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        for (int i = 0; i < HAND_COUNT; ++i)
        {
            _modelHandTransforms[i].position = _ovrCameraHandTransforms[i].position;
            _modelHandTransforms[i].rotation = _ovrCameraHandTransforms[i].rotation;
        }
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
        foreach (OVRRaycaster canvas in ovrRaycasters)
        {
            canvas.pointer = _pointer;
        }
    }
}

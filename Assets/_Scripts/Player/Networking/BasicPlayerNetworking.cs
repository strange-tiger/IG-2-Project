using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Asset.MySql;

public class BasicPlayerNetworking : MonoBehaviourPunCallbacks
{
    [SerializeField] protected Vector3 _ovrCameraPosition = new Vector3(0f, 0.7f, 0.8f);
    [SerializeField] protected GameObject _ovrCameraRigPrefab;
    protected GameObject _myOVRCameraRig;

    protected const int HAND_COUNT = 2;
    [SerializeField] protected Transform[] _modelHandTransforms;
    protected Transform[] _ovrCameraHandTransforms = new Transform[2];

    [SerializeField] protected TextMeshProUGUI _nicknameText;

    protected GameObject _pointer;

    public string MyNickname { get; private set; }
    public string MyUserId { get; private set; }

    protected virtual void Awake()
    {
        if (photonView.IsMine)
        {
            // CameraRig ����
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            _myOVRCameraRig = cameraRig;
            cameraRig.transform.localPosition = _ovrCameraPosition;

            // ���� �޾ƿ� ������
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            _ovrCameraHandTransforms[0] = hands[0].transform.parent.GetChild(0);
            _ovrCameraHandTransforms[1] = hands[1].transform.parent.GetChild(0);

            // ���� ���� canvas�� �����ϱ� ���� ������ ��������
            _pointer = cameraRig.GetComponentInChildren<OVRGazePointer>().gameObject;

            
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 1);

            
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

    private void OnDestroy()
    {
        if (photonView.IsMine)
        {
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);
        }
    }
}

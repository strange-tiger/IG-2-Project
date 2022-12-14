using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Asset.MySql;
using MapType = Defines.EMapType;

public class BasicPlayerNetworking : PlayerHandRigging
{
    [SerializeField] protected Vector3 _ovrCameraPosition = new Vector3(0f, 0.7f, 0.8f);
    [SerializeField] protected GameObject _ovrCameraRigPrefab;
    protected GameObject _myOVRCameraRig;

    protected const int HAND_COUNT = 2;
    protected Transform[] _ovrCameraHandTransforms = new Transform[2];

    [SerializeField] protected TextMeshProUGUI _nicknameText;

    protected GameObject _pointer;

    protected MapPanelManager _mapManager;

    public string MyNickname { get; private set; }
    public string MyUserId { get; private set; }

    protected virtual void Awake()
    {
        if (photonView.IsMine)
        {
            // CameraRig 부착
            GameObject cameraRig = Instantiate(_ovrCameraRigPrefab, gameObject.transform);
            _myOVRCameraRig = cameraRig;
            cameraRig.transform.localPosition = _ovrCameraPosition;

            // 손을 받아와 연결함
            PlayerFocus[] hands = cameraRig.GetComponentsInChildren<PlayerFocus>();
            _ovrCameraHandTransforms[0] = hands[0].transform.parent.GetChild(0);
            _ovrCameraHandTransforms[1] = hands[1].transform.parent.GetChild(0);

            // 월드 내의 canvas와 연결하기 위한 포인터 가져오기
            _pointer = cameraRig.GetComponentInChildren<OVRGazePointer>().gameObject;

            _mapManager = cameraRig.GetComponentInChildren<MapPanelManager>();
        }
    }

    protected override void FixedUpdate()
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

    public void SetMap(MapType mapType, bool isFixedPosition, Vector3 fixedPosition, Vector3 fixedRotation)
    {
        if(!_mapManager)
        { 
            return;
        }

        _mapManager.SetMap(mapType);
        if(isFixedPosition)
        {
            _mapManager.SetFixedPlayerPosition(fixedPosition, fixedRotation);
        }
    }
}

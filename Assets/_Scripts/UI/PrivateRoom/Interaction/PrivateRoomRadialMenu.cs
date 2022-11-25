using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PrivateRoomRadialMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _privateRoomRadialMenu;
    [SerializeField] Image _privateRoomRadialCursor;
    [SerializeField] GameObject _dice;
    [SerializeField] GameObject _paintbrush;
    [SerializeField] Button _buttonDice;

    [Header("Attach")]
    [SerializeField] Canvas _canvas;

    private static readonly YieldInstruction MENU_DELAY = new WaitForSeconds(2f);

    public static Button ClickButton;

    private SpawnDice _spawnDice;
    private SpawnPaintbrush _spawnPaintbrush;

    private NewPlayerMove _playerMove;

    private Vector2 _priavteRoomRadialCursorInitPosition;
    private float _privateRoomRadialCursorMovementLimit = 25f;
    private float _privateRoomRadialCursorSpeed = 100f;

    private void Start()
    {
        _priavteRoomRadialCursorInitPosition = _privateRoomRadialCursor.rectTransform.localPosition;

        PrivateRoomEnterance();

        StartCoroutine(FindCamera());
    }

    private const string EYE_CAMERA = "CenterEyeAnchor";
    IEnumerator FindCamera()
    {
        yield return MENU_DELAY;

        GameObject findCamera = GameObject.Find(EYE_CAMERA);

        Debug.Assert(findCamera != null, "카메라 찾기 실패");
        
        if (photonView.IsMine)
        {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = findCamera.GetComponent<Camera>();
            _canvas.planeDistance = 2f;
        }

        _playerMove = findCamera.transform.root.GetComponent<NewPlayerMove>();
    }

    private static readonly Vector3 INSTANTIATE_POS = new Vector3(0f, 2f, 0f);

    private void PrivateRoomEnterance()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _dice = PhotonNetwork.Instantiate("PrivateRoom\\Dice", INSTANTIATE_POS, transform.rotation);

            _spawnDice = _dice.GetComponent<SpawnDice>();
            _spawnDice.SetPlayerTransform(transform);
        }
        else
        {
            _buttonDice.interactable = false;
        }

        _paintbrush = PhotonNetwork.Instantiate("PrivateRoom\\Paintbrush", INSTANTIATE_POS, transform.rotation);

        _spawnPaintbrush = _paintbrush.GetComponent<SpawnPaintbrush>();
        _spawnPaintbrush.SetPlayerTransform(transform);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            _buttonDice.interactable = true;

            _dice = PhotonNetwork.Instantiate("PrivateRoom\\Dice", INSTANTIATE_POS, transform.rotation);

            _spawnDice = _dice.GetComponent<SpawnDice>();
            _spawnDice.SetPlayerTransform(transform);
        }
    }

    private const string CALL_METHOD = "CallMethod";
    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _privateRoomRadialMenu.SetActive(true);
            _playerMove.enabled = false;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
        {
            photonView.RPC(CALL_METHOD, RpcTarget.All);
            _playerMove.enabled = true;
        }
        else
        {
            _privateRoomRadialMenu.SetActive(false);
        }
    }

    private const string BUTTON_A = "ButtonA";
    [PunRPC]
    private void CallMethod()
    {
        if (ClickButton.name == BUTTON_A)
        {
            ButtonAMethod();
        }
        else
        {
            ButtonBMethod();
        }
    }

    private void ButtonAMethod()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        _spawnDice.ToggleDice();
    }

    private void ButtonBMethod()
    {
        if (!_spawnPaintbrush.photonView.IsMine)
        {
            return;
        }
        _spawnPaintbrush.TogglePaintbrush();
    }

    void FixedUpdate()
    {
        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            MoveCursor();
        }
        else
        {
            ResetCursor();
        }
    }

    void MoveCursor()
    {
        Vector3 direction = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);


        direction.Normalize();


        _privateRoomRadialCursor.rectTransform.localPosition = Vector3.ClampMagnitude(_privateRoomRadialCursor.rectTransform.localPosition + direction * _privateRoomRadialCursorSpeed * Time.deltaTime, _privateRoomRadialCursorMovementLimit);

    }

    void ResetCursor()
    {
        _privateRoomRadialCursor.rectTransform.localPosition = _priavteRoomRadialCursorInitPosition;
    }
}

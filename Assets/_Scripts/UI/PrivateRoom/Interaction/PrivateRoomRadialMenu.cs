#define _VR

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
    //[SerializeField] 
    [SerializeField] GameObject _paintbrush;
    [SerializeField] Button _buttonDice;

    [Header("Attach")]
    [SerializeField] Canvas _canvas;

    private static readonly YieldInstruction MENU_DELAY = new WaitForSeconds(2f);

    public static Button ClickButton;

    private SpawnDice _spawnDice;
    private SpawnPaintbrush _spawnPaintbrush;

    private Vector2 _priavteRoomRadialCursorInitPosition;
    private float _privateRoomRadialCursorMovementLimit = 25f;
    private float _privateRoomRadialCursorSpeed = 100f;

    private void Start()
    {
        _priavteRoomRadialCursorInitPosition = _privateRoomRadialCursor.rectTransform.localPosition;

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;

        StartCoroutine(FindCamera());
    }

    IEnumerator FindCamera()
    {
        yield return MENU_DELAY;

        GameObject findCamera = GameObject.Find("CenterEyeAnchor");

        Debug.Assert(findCamera != null, "카메라 찾기 실패");

        if (photonView.IsMine)
        {
            _canvas.worldCamera = findCamera.GetComponent<Camera>();
            _canvas.planeDistance = 1f;
        }
    }

    private static readonly Vector3 INSTANTIATE_POS = new Vector3(0f, 2f, 0f);

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("성공");

        if (PhotonNetwork.IsMasterClient)
        {
            _dice = PhotonNetwork.Instantiate("PrivateRoom\\Dice", INSTANTIATE_POS, transform.rotation);
        }
        else
        {
            _buttonDice.interactable = false;
        }

        _paintbrush = PhotonNetwork.Instantiate("PrivateRoom\\Paintbrush", INSTANTIATE_POS, transform.rotation);

        _spawnDice = _dice.GetComponent<SpawnDice>();
        _spawnPaintbrush = _paintbrush.GetComponent<SpawnPaintbrush>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

#if _VR
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick))
        {
            _privateRoomRadialMenu.SetActive(true);
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick))
        {
            photonView.RPC("CallMethod", RpcTarget.All);
        }
        else
        {
            _privateRoomRadialMenu.SetActive(false);
        }
#else
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(ClickButton.name); 
            CallMethod();
        }
#endif
    }

    [PunRPC]
    private void CallMethod()
    {
        Debug.Log("Call");
        if (ClickButton.name == "ButtonA")
        {
            ButtonAMethod();
        }
        else if (ClickButton.name == "ButtonB")
        {
            ButtonBMethod();
        }
        else
        {
            ButtonCMethod();
        }
    }

    private void ButtonAMethod()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        Debug.Log("dice");
        _spawnDice.ToggleDice();
    }

    private void ButtonBMethod()
    {

    }

    private void ButtonCMethod()
    {
        if (!_spawnPaintbrush.photonView.IsMine)
        {
            return;
        }
        Debug.Log("paint");
        _spawnPaintbrush.SpawnHelper();
    }

    void FixedUpdate()
    {
#if _VR
        if (OVRInput.Get(OVRInput.Touch.SecondaryThumbstick))
        {
            MoveCursor();
        }
        else
        {
            ResetCursor();
        }
#else
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            MoveCursor();
        }
        else
        {
            ResetCursor();
        }
#endif
    }


    void MoveCursor()
    {
#if _VR
        Vector3 direction = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
#else
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
#endif

        direction.Normalize();


        _privateRoomRadialCursor.rectTransform.localPosition = Vector3.ClampMagnitude(_privateRoomRadialCursor.rectTransform.localPosition + direction * _privateRoomRadialCursorSpeed * Time.deltaTime, _privateRoomRadialCursorMovementLimit);

    }

    void ResetCursor()
    {
        _privateRoomRadialCursor.rectTransform.localPosition = _priavteRoomRadialCursorInitPosition;
    }
}

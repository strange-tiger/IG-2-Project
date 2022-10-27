using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PrivateRoomRadialMenu : MonoBehaviourPun
{
    [SerializeField] GameObject _privateRoomRadialMenu;
    [SerializeField] Image _privateRoomRadialCursor;
    public static Button ClickButton;

    private Vector2 _priavteRoomRadialCursorInitPosition;
    private float _privateRoomRadialCursorMovementLimit = 25f;
    private float _privateRoomRadialCursorSpeed = 100f;

    private SpawnDice _dice;
    //private 
    private SpawnPaintbrush _paintbrush;

    private void Start()
    {
        _priavteRoomRadialCursorInitPosition = _privateRoomRadialCursor.rectTransform.localPosition;

        _dice = GetComponent<SpawnDice>();
        //
        _paintbrush = GetComponent<SpawnPaintbrush>();
    }

    private void Update()
    {
        /*
        if (!photonView.IsMine)
        {
            return;
        }

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
        */
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(ClickButton.name); 
            CallMethod();
        }
    }

    [PunRPC]
    private void CallMethod()
    {
        Debug.Log("Call");
        if(ClickButton.name == "ButtonA")
        {
            ButtonAMethod();
        }
        else if(ClickButton.name == "ButtonB")
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
        //if(!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}
        Debug.Log("dice");
        _dice.ToggleDice();
    }

    private void ButtonBMethod()
    {

    }

    private void ButtonCMethod()
    {
        Debug.Log("paint");
        _paintbrush.Spawn();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
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
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        direction.Normalize();


        _privateRoomRadialCursor.rectTransform.localPosition = Vector3.ClampMagnitude(_privateRoomRadialCursor.rectTransform.localPosition + direction * _privateRoomRadialCursorSpeed * Time.deltaTime, _privateRoomRadialCursorMovementLimit);

    }

    void ResetCursor()
    {
        _privateRoomRadialCursor.rectTransform.localPosition = _priavteRoomRadialCursorInitPosition;
    }
}

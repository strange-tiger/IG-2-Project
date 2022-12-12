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

    private static readonly YieldInstruction MENU_DELAY = new WaitForSeconds(1f);

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
    /// <summary>
    /// 이 오브젝트의 캔버스 _canvas의 설정을 변경하고 
    /// 각 월드에서 각 플레이어만 갖는 카메라를 찾아 _canvas의 worldCamera에 할당한다.
    /// 메뉴가 활성화되면 플레이어의 움직임을 멈추기 위해 _playerMove 또한 할당한다.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 유저가 사설 공간에 처음 들어가고 사설 공간 씬이 로드될 때, 
    /// 유저가 마스터 클라이언트라면 주사위를 생성하고, 
    /// 각 유저가 소유하는 그림판을 생성한다.
    /// </summary>
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

    /// <summary>
    /// 룸의 마스터 클라이언트가 바뀔 때 호출된다.
    /// 주사위를 생성한다.
    /// </summary>
    /// <param name="newMasterClient"></param>
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
    /// <summary>
    /// 매 프레임 오른손 조이스틱에 버튼 입력을 검사한다.
    /// 입력 여부에 따라 메뉴의 활성화 여부와 플레이어 움직임 활성화 여부를 결정한다.
    /// GetUp 메소드로 CallMethod를 RPC로 호출한다.
    /// </summary>
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
    /// <summary>
    /// 메뉴의 커서가 ButtonA에 충돌했는가 아닌가로 ButtonAMethod와 ButtonBMethod 중 어느 메소드를 호출할 지 결정한다.
    /// </summary>
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

    /// <summary>
    /// 현재 이 클라이언트가 마스터 클라이언트라면 생성했던 주사위의 ToggleDice를 호출한다.
    /// </summary>
    private void ButtonAMethod()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        _spawnDice.ToggleDice();
    }

    /// <summary>
    /// 그림판의 TogglePaintbrush를 호출한다.
    /// </summary>
    private void ButtonBMethod()
    {
        if (!_spawnPaintbrush.photonView.IsMine)
        {
            return;
        }
        _spawnPaintbrush.TogglePaintbrush();
    }

    /// <summary>
    /// 매 프레임 오른손 조이스틱의 터치 입력을 검사한다.
    /// 입력에 따라 커서를 움직이고 입력이 없다면 커서를 원위치로 되돌린다.
    /// </summary>
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

    /// <summary>
    /// 오른손 조이스틱의 터치 입력에 따라 커서 오브젝트를 움직인다.
    /// </summary>
    void MoveCursor()
    {
        Vector3 direction = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);


        direction.Normalize();


        _privateRoomRadialCursor.rectTransform.localPosition = Vector3.ClampMagnitude(_privateRoomRadialCursor.rectTransform.localPosition + direction * _privateRoomRadialCursorSpeed * Time.deltaTime, _privateRoomRadialCursorMovementLimit);

    }

    /// <summary>
    /// 커서를 원위치로 되돌린다.
    /// </summary>
    void ResetCursor()
    {
        _privateRoomRadialCursor.rectTransform.localPosition = _priavteRoomRadialCursorInitPosition;
    }
}

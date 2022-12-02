using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using System;


public class RadialMenu : MonoBehaviourPun, IPunObservable
{
    // 플레이어의 OVRCamearaRig에 있는 RadialMenu
    [Header("Radial Menu")]
    [SerializeField] private RectTransform _radialMenu;
    [SerializeField] private Image _cursor;

    // 선택할 수 있는 버튼과 이미지의 배열
    [Header("Selectable Button")]
    [SerializeField] private Button[] _buttonOnes;
    [SerializeField] private Sprite[] _buttonOneImages;

    // 선택된 이미지를 표현함
    [Header("Feeling Image")]
    [SerializeField] private Image _feelingImage;

    // 선택된 버튼과 이미지
    public static Button _buttonOne;
    public static Image _buttonOneImage;

    private static readonly YieldInstruction _waitSecond = new WaitForSeconds(0.0001f);

    // 커서의 움직임에 관련된 변수
    private Vector2 _cursorInitPosition;
    private float _cursorMovementLimit = 60f;
    private float _cursorSpeed = 120f;

    // 버튼의 인덱스, 페이드아웃되는 시간과, 변화하는 이미지의 컬러 데이터
    private int _buttonIndex;
    private float _coolTime = 4f;
    private float _colorData;

    /// <summary>
    /// 머리위의 이미지의 알파값과 띄울 이미지의 Index를 직렬화하여 전송.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_colorData);
            stream.SendNext(_buttonIndex);
        }
        else if (stream.IsReading)
        {
            _feelingImage.color = new Color(1, 1, 1, (float)stream.ReceiveNext());
            _feelingImage.sprite = _buttonOneImages[(int)stream.ReceiveNext()];
        }
    }

    /// <summary>
    /// OVRCamera의 VRUI에 있는 RadialMenu를 찾아오고, 각 버튼과 커서를 초기화함.
    /// </summary>
    private void Start()
    {
        if (photonView.IsMine)
        {
            _radialMenu = GameObject.Find("RadialMenu").GetComponent<RectTransform>();
            for (int i = 0; i < _radialMenu.childCount - 1; ++i)
            {
                _buttonOnes[i] = _radialMenu.GetChild(i).GetComponent<Button>();
            }
            _cursor = _radialMenu.GetChild(_radialMenu.childCount - 1).GetComponent<Image>();
            _cursorInitPosition = _cursor.rectTransform.localPosition;
            _radialMenu.gameObject.SetActive(false);
        }
    }

    
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                // 왼쪽 스틱 버튼을 눌러 UI 활성화.
                _radialMenu.gameObject.SetActive(true);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick))
            {
                // 떼었으면 해당 이미지를 띄우는 RPC 메서드를 호출.
                photonView.RPC("ButtonOneMenu", RpcTarget.All);

            }
            else
            {
                // 이외의 경우 현재 적용된 버튼의 정보를 초기화 한후
                if (_buttonOne != null)
                {
                    _buttonOne.transform.GetChild(1).gameObject.SetActive(false);
                    _buttonOne = null;
                    _buttonOneImage = null;
                }

                // UI를 비활성화 시킴.
                _radialMenu.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 선택한 버튼의 이미지를 띄울 RPC 메서드.
    /// </summary>
    [PunRPC]
    public void ButtonOneMenu()
    {
        if (_buttonOne != null)
        {
            // 알파값을 조절하여 이미지를 띄움.
            _colorData = 1;

            _feelingImage.color = new Color(1, 1, 1, _colorData);

            // 선택한 버튼과 일치하는 인덱스를 찾아냄.
            if (photonView.IsMine)
            {
                for (int i = 0; i < _buttonOnes.Length; ++i)
                {
                    if (_buttonOnes[i].name == _buttonOne.name)
                    {
                        _buttonIndex = i;
                    }
                }
            }
            // 이미지를 적용시키고
            _feelingImage.sprite = _buttonOneImages[_buttonIndex];
            // UI를 비활성화 시킴
            _radialMenu.gameObject.SetActive(false);

            _elapsedTime = 0f;

            // 페이드아웃이 진행중이 아니라면 코루틴을 재실행하고
            if (_isFadeRunning == false)
            {
                StartCoroutine(CoFade(1, 0));
            }
            else
            {
                // 아니라면 이미지가 변경되고, 알파값을 초기화시켜 처음부터 페이드아웃 되게함.
                _isFadeRestart = true;
            }
        }
    }

    private bool _isFadeRestart;
    private bool _isFadeRunning;
    float animatedFadeAlpha;
    float _elapsedTime = 0.0f;

    /// <summary>
    /// 머리 위에 띄울 이미지를 페이드 아웃 시키는 메서드.
    /// </summary>
    /// <param name="startAlpha"></param>
    /// <param name="endAlpha"></param>
    /// <returns></returns>
    private IEnumerator CoFade(float startAlpha, float endAlpha)
    {
        // 페이드 아웃이 진행중임을 Boolean 값으로 표현
        _isFadeRunning = true;

        // 선형 보간을 이용한 이미지 페이드 아웃
        while (_elapsedTime < _coolTime)
        {
            _elapsedTime += Time.deltaTime;

            animatedFadeAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(_elapsedTime / _coolTime));

            _colorData = animatedFadeAlpha;

            _feelingImage.color = new Color(1, 1, 1, _colorData);

            // 다른 감정표현을 선택하면 다시 알파값을 1로 만들어 이미지만 변경하여 처음부터 페이드아웃되도록 함.
            if (!_isFadeRestart)
            {
                animatedFadeAlpha = 1f;
                _isFadeRestart = false;
            }

            yield return _waitSecond;
        }

        _colorData = endAlpha;

        _feelingImage.color = new Color(1, 1, 1, _colorData);

        // 페이드 아웃이 끝남을 알림.
        _isFadeRunning = false;
    }

    /// <summary>
    /// 플레이어의 입력에 따라 Cursor를 움직이거나, 위치를 초기화 시킴.
    /// </summary>
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(OVRInput.Touch.PrimaryThumbstick))
            {
                MoveCursor();

            }
            else
            {
                ResetCursor();
            }
        }
    }

    /// <summary>
    /// Cursor를 움직이는 메서드
    /// </summary>
    private void MoveCursor()
    {
        // Cursor의 움직임은 왼쪽스틱으로 움직임
        Vector3 direction = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        direction.Normalize();

        // Cursor가 특정 지름 크기 내에서만 움직이도록 해줌.
        _cursor.rectTransform.localPosition = Vector3.ClampMagnitude(_cursor.rectTransform.localPosition + direction * _cursorSpeed * Time.deltaTime, _cursorMovementLimit);
    }

    /// <summary>
    /// UI가 비활성화되거나, 스틱의 입력값이 없는 경우 Cursor는 초기 위치로 돌아감.
    /// </summary>
    private void ResetCursor()
    {
        _cursor.rectTransform.localPosition = _cursorInitPosition;
    }

}

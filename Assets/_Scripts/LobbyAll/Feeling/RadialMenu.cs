using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;
using System;


public class RadialMenu : MonoBehaviourPun, IPunObservable
{


    [SerializeField] RectTransform _radialMenu;
    [SerializeField] Image _cursor;
    [SerializeField] Image _feelingImage;
    [SerializeField] Sprite[] _buttonOneImages;
    [SerializeField] Button[] _buttonOnes;
    public static Button _buttonOne;
    public static Image _buttonOneImage;

    private static readonly YieldInstruction _waitSecond = new WaitForSeconds(0.0001f);
    private Vector2 _cursorInitPosition;
    private float _cursorMovementLimit = 45f;
    private float _cursorSpeed = 100f;
    private float _coolTime = 4f;
    private float _colorData;
    private int _buttonIndex;

    private void Start()
    {
        if(photonView.IsMine)
        {
            _radialMenu = GameObject.Find("RadialMenu").GetComponent<RectTransform>();
            for(int i = 0; i < _radialMenu.childCount - 1; ++i)
            {
                _buttonOnes[i] = _radialMenu.GetChild(i).GetComponent<Button>();
            }
            _cursor = _radialMenu.GetChild(_radialMenu.childCount - 1).GetChild(0).GetComponent<Image>();
            _cursorInitPosition = _cursor.rectTransform.localPosition;
            _radialMenu.gameObject.SetActive(false);
        }
    }
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_colorData);
            stream.SendNext(_buttonIndex);
        }
        else if (stream.IsReading)
        {
            _feelingImage.color = new Color(1,1,1,(float)stream.ReceiveNext());
            _feelingImage.sprite = _buttonOneImages[(int)stream.ReceiveNext()];
        }
    }
    private void Update()
    {
        if (photonView.IsMine)
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
            {
                _radialMenu.gameObject.SetActive(true);
            }
            else if (OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick))
            {
                photonView.RPC("ButtonOneMenu", RpcTarget.All);
                
            }
            else
            {
                _radialMenu.gameObject.SetActive(false);
            }

        }

        
        
    }

    private bool _isFadeRunning;
    float _elapsedTime = 0.0f;
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        _isFadeRunning = true;
        while (_elapsedTime < _coolTime)
        {
            _elapsedTime += Time.deltaTime;
            float animatedFadeAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(_elapsedTime / _coolTime));
            _colorData = animatedFadeAlpha;
            _feelingImage.color = new Color(1, 1, 1, _colorData);
            yield return _waitSecond;
        }
        _colorData = endAlpha;
        _feelingImage.color = new Color(1, 1, 1, _colorData);

        _isFadeRunning = false;
        yield return null;

    }

    [PunRPC]
    public void ButtonOneMenu()
    {
       
            if (_buttonOne != null)
            {

                _colorData = 1;
                _feelingImage.color = new Color(1, 1, 1, _colorData);
                if (photonView.IsMine)
                {
                    for (int i = 0; i < _buttonOnes.Length - 1; ++i)
                    {
                        if (_buttonOnes[i].name == _buttonOne.name)
                        {
                            _buttonIndex = i;
                        }
                    }
                }
                _feelingImage.sprite = _buttonOneImages[_buttonIndex];

                _radialMenu.gameObject.SetActive(false);


                _elapsedTime = 0f;

                if (_isFadeRunning == false)
                {
                    StartCoroutine(Fade(1, 0));
                }
            }
        

    }

    void FixedUpdate()
    {
        if(photonView.IsMine)
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


    void MoveCursor()
    {
        Vector3 direction = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        direction.Normalize();


        _cursor.rectTransform.localPosition = Vector3.ClampMagnitude(_cursor.rectTransform.localPosition + direction * _cursorSpeed * Time.deltaTime, _cursorMovementLimit);

    }

    void ResetCursor()
    {
        _cursor.rectTransform.localPosition = _cursorInitPosition;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

public class BeerInteraction : MonoBehaviourPun, IPunObservable
{

    [SerializeField] Material _fadeMaterial;

    private PlayerControllerMove _playerContollerMove;
    private MeshRenderer _fadeRenderer;
    private YieldInstruction _coolTime = new WaitForSeconds(10f);
    private YieldInstruction _fadeTime = new WaitForSeconds(0.0001f);
    private YieldInstruction _stunTime = new WaitForSeconds(5f);
    private Color _initUIColor = new Color(1f, 1f, 0.28f, 0f);
    private int _drinkStack = -1;
    private bool _isTrembling;
    private bool _isCoolTime;
    private float[] _tremblingSpeed = new float[2];
    private float _soberUpElapsedTime;
    private float _tremblingElapsedTime;
    private float _initPlayerSpeed = 1.0f;
    private float _animatedFadeAlpha;
   
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(_drinkStack);
        }
        else if (stream.IsReading)
        {
            _drinkStack = (int)stream.ReceiveNext();
        }
    }


    private void Start()
    {
        _playerContollerMove = GetComponentInParent<PlayerControllerMove>();

        StartCoroutine(FindFadeImage());
    }

    IEnumerator FindFadeImage()
    {
        yield return new WaitForSeconds(4f);

        _fadeRenderer = GameObject.Find("CenterEyeAnchor").GetComponent<MeshRenderer>();
        _fadeRenderer.material = _fadeMaterial;
        _fadeRenderer.enabled = true;

        yield return null; 
    }
    private void Update()
    {

        if(_drinkStack > -1)
        {
            _soberUpElapsedTime += Time.deltaTime;

            if(_soberUpElapsedTime >= 60)
            {
                SoberUp();
            }

            _tremblingElapsedTime += Time.deltaTime;

            if (_tremblingElapsedTime > 5f && _drinkStack > 0)
            {
                _isTrembling = !_isTrembling;

                _tremblingElapsedTime = 0f;


                if (_isTrembling)
                {
                    _playerContollerMove.MoveScale = _tremblingSpeed[0];
                }
                else
                {
                    _playerContollerMove.MoveScale = _tremblingSpeed[1];
                }
            }
        }
    }

    [PunRPC]
    public void CallDrinkBeer()
    {
        if(photonView.IsMine)
        {
            DrinkBeer();
        }
    }
    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        float fadeTime = 3f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            _animatedFadeAlpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.Clamp01(elapsedTime / fadeTime));

            _fadeMaterial.color = new Color(0, 0, 0, _animatedFadeAlpha);

            yield return _fadeTime;
        }

        _animatedFadeAlpha = endAlpha;

        PlayerControlManager.Instance.IsMoveable = false;
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsInvincible = true;

        yield return _stunTime;

        PlayerControlManager.Instance.IsMoveable = true;
        PlayerControlManager.Instance.IsRayable = true;
        PlayerControlManager.Instance.IsInvincible = false;

        elapsedTime = 0.0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;

            _animatedFadeAlpha = Mathf.Lerp(endAlpha, startAlpha, Mathf.Clamp01(elapsedTime / fadeTime));

            _fadeMaterial.color = new Color(0, 0, 0, _animatedFadeAlpha);

            yield return _fadeTime;
        }

        _animatedFadeAlpha = startAlpha;

        _drinkStack = -1;

        _fadeMaterial.color = _initUIColor;

        _tremblingSpeed[0] = _initPlayerSpeed;
        _tremblingSpeed[1] = _initPlayerSpeed;

    }
    private void DrinkBeer()
    {
        if(photonView.IsMine)
        {
            _isCoolTime = true;

            _drinkStack++;

            _soberUpElapsedTime = 0;


            _playerContollerMove.MoveScale = _initPlayerSpeed;

            StartCoroutine(CoolTime());

            if(_drinkStack == 5)
            {
               StartCoroutine(Fade(0, 1));   
            }
            else if(_drinkStack < 5 && _drinkStack > 0)
            {
                _fadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));

                _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
                _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            }

        }
        
    }

  
    private void SoberUp()
    {
        if(photonView.IsMine)
        {
            _drinkStack--;

            _soberUpElapsedTime = 0;

            _fadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));

            _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
        }
    }

    private IEnumerator CoolTime()
    {
        yield return _coolTime;

        _isCoolTime = false;

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Beer"))
        {
            if(!_isCoolTime)
            {
                photonView.RPC("CallDrinkBeer",RpcTarget.All);
                other.GetComponent<Beer>().CallDrinkBeer();
                Debug.Log(_drinkStack);
            }
        }
    }


}

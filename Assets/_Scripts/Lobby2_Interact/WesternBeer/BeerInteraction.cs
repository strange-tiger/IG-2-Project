using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

public class BeerInteraction : MonoBehaviourPun, IPunObservable
{
    private YieldInstruction _coolTime = new WaitForSeconds(10f);

    private PlayerControllerMove _playerContollerMove;

    private PlayerDebuffManager _playerDebuff;

    private Color _initUIColor = new Color(1f, 1f, 0.28f, 0f);

    private int _drinkStack = -1;

    private bool _isCoolTime;
    private bool _isTrembling;
    private float[] _tremblingSpeed = new float[2];

    private float _soberUpElapsedTime;
    private float _tremblingElapsedTime;

    private float _initPlayerSpeed = 1.0f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
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
        _playerDebuff = GetComponentInParent<PlayerDebuffManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Beer"))
        {
            if (!_isCoolTime)
            {
                photonView.RPC("CallDrinkBeer", RpcTarget.All);
                other.GetComponentInParent<Beer>().CallDrinkBeer();
                Debug.Log(_drinkStack);
            }
        }
    }

    private void Update()
    {
        if (_drinkStack > -1)
        {
            _soberUpElapsedTime += Time.deltaTime;

            if (_soberUpElapsedTime >= 60)
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

    private void SoberUp()
    {
        if (photonView.IsMine)
        {
            _drinkStack--;

            _soberUpElapsedTime = 0;

            _playerDebuff.FadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));

            _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
        }
    }

    [PunRPC]
    public void CallDrinkBeer()
    {
        if (photonView.IsMine)
        {
            DrinkBeer();
        }
    }

    private void DrinkBeer()
    {
        if (photonView.IsMine)
        {
            _isCoolTime = true;

            _drinkStack++;

            _soberUpElapsedTime = 0;


            _playerContollerMove.MoveScale = _initPlayerSpeed;

            StartCoroutine(CoCoolTime());

            if (_drinkStack == 5)
            {
                DrunkenDebuff();
            }
            else if (_drinkStack < 5 && _drinkStack > 0)
            {
                _playerDebuff.FadeMaterial.color = new Color(1f, 1f, 0.28f, (0f + (0.1f * _drinkStack)));

                _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
                _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            }
        }
    }
    private IEnumerator CoCoolTime()
    {
        yield return _coolTime;

        _isCoolTime = false;

    }
    private void DrunkenDebuff()
    {
        _playerDebuff.CallDrunkenDebuff();

        _drinkStack = -1;

        _playerDebuff.FadeMaterial.color = _initUIColor;

        _tremblingSpeed[0] = _initPlayerSpeed;
        _tremblingSpeed[1] = _initPlayerSpeed;
    }
}

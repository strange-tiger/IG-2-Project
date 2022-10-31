using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Photon.Pun;

public class BeerInteraction : MonoBehaviourPun
{

    public UnityEvent OnDrunkenBeer = new UnityEvent();
    public bool IsCoolTime { get; private set; }

    [SerializeField] Image _drunkenUI;

    private PlayerControllerMove _playerContollerMove;
    private PlayerInput _playerInput;
    private YieldInstruction _coolTime = new WaitForSeconds(10f);
    private int _drinkStack = -1;
    private float _soberUpElapsedTime;
    private float _tremblingElapsedTime;
    private float _initPlayerSpeed;
    private bool _isTrembling;
    private float[] _tremblingSpeed;
    private Color _initUIColor = new Color(1f, 1f, 0.28f, 1f);
    private void OnEnable()
    {
        Beer.OnDrinkBeer.RemoveListener(CallDrinkBeer);
        Beer.OnDrinkBeer.AddListener(CallDrinkBeer);

        _playerContollerMove = GetComponentInParent<PlayerControllerMove>();

        _playerInput = GameObject.Find("OVRCamerarig").GetComponent<PlayerInput>();

        _initPlayerSpeed = _playerContollerMove.MoveScale;
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

            if (_tremblingElapsedTime > 5f)
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

    public void CallDrinkBeer()
    {
        if(!IsCoolTime)
        {
            DrinkBeer();
        }
    }

    private void DrinkBeer()
    {
        IsCoolTime = true;

        _drinkStack++;

        _soberUpElapsedTime = 0;

        _playerContollerMove.MoveScale = _initPlayerSpeed;

        StartCoroutine(CoolTime());

        if(_drinkStack == 5)
        {

            _drinkStack = -1;

            _drunkenUI.color = _initUIColor;

            _tremblingSpeed[0] = _initPlayerSpeed;
            _tremblingSpeed[1] = _initPlayerSpeed;
        }
        else if(_drinkStack < 5 && _drinkStack > 0)
        {
            _drunkenUI.color = new Color(1f, 1f, 0.28f, (1f - (0.1f * _drinkStack)));

            _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
            _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
        }
        
    }

  
    private void SoberUp()
    {
        _drinkStack--;

        _soberUpElapsedTime = 0;

        _drunkenUI.color = new Color(1f, 1f, 0.28f, (1f - (0.1f * _drinkStack)));

        _tremblingSpeed[0] = _initPlayerSpeed + (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
        _tremblingSpeed[1] = _initPlayerSpeed - (_playerContollerMove.MoveScale * (0.05f * _drinkStack));
    }

    private IEnumerator CoolTime()
    {
        yield return _coolTime;

        IsCoolTime = false;

        yield return null;
    }

    private void OnDisable()
    {
        Beer.OnDrinkBeer.RemoveListener(CallDrinkBeer);
    }



}

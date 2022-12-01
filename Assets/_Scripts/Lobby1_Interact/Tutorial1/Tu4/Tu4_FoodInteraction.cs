using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tu4_FoodInteraction : MonoBehaviour
{

    public UnityEvent OnActivateSatietyUI = new UnityEvent();
    public UnityEvent OnChangeSatietyUI = new UnityEvent();
    public UnityEvent OnDeactivateSatietyUI = new UnityEvent();
    public int SatietyStack { get; private set; }

    private PlayerControllerMove _playerContollerMove;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _eatingSound;

    private Vector3 _initPosition;
    private Vector3 _nullPosition = new Vector3(0, 0, 0);

    private float _speedSlower = 0.0001f;
    private float _fatterCharacter = 0.1f;

    private float _walkCount;
    private int _dietWalkCount = 100;

    private int _maxSatietyStack = 6;

    private void OnEnable()
    {
        Tu4_Foods.OnEatedTu.RemoveListener(EatFoodTu);
        Tu4_Foods.OnEatedTu.AddListener(EatFoodTu);

        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        _playerContollerMove = GetComponentInParent<PlayerControllerMove>();
    }

    private void Update()
    {
        if (SatietyStack != 0)
        {
            Diet();
        }
    }

    private void Diet()
    {
        if (_initPosition == _nullPosition)
        {
            _initPosition = transform.position;
        }

        if (Vector3.Distance(_initPosition, transform.position) >= 1)
        {
            _walkCount++;
            _initPosition = transform.position;
        }

        if (_walkCount == _dietWalkCount)
        {
            SatietyStack--;

            _walkCount = 0;

            _playerContollerMove.MoveScale += _speedSlower;

            CharacterScaleDecrease();

            if (SatietyStack == 0)
            {
                OnDeactivateSatietyUI.Invoke();
                _initPosition = _nullPosition;
            }
            else
            {
                OnChangeSatietyUI.Invoke();
            }
        }
    }

    private void EatFoodTu(EFoodSatietyLevels foodSatietyLevel)
    {
        if (SatietyStack < _maxSatietyStack)
        {
            _playerContollerMove.MoveScale -= _speedSlower * (int)foodSatietyLevel;

            CharacterScaleIncrease(foodSatietyLevel);

            if (SatietyStack == 0)
            {
                OnActivateSatietyUI.Invoke();
            }

            SatietyStack += (int)foodSatietyLevel;

            if (SatietyStack > _maxSatietyStack)
            {
                SatietyStack = _maxSatietyStack;
            }

            OnChangeSatietyUI.Invoke();
        }
    }

    public void CharacterScaleIncrease(EFoodSatietyLevels foodSatietyLevel)
    {
        _audioSource.PlayOneShot(_eatingSound);

        transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, transform.GetChild(2).localScale.y, transform.GetChild(2).localScale.z + (_fatterCharacter * (int)foodSatietyLevel));
    }

    public void CharacterScaleDecrease()
    {
        transform.GetChild(2).localScale = new Vector3(transform.GetChild(2).localScale.x, transform.GetChild(2).localScale.y, transform.GetChild(2).localScale.z - _fatterCharacter);
    }

    private void OnDisable()
    {
        Tu4_Foods.OnEatedTu.RemoveListener(EatFoodTu);
    }

}

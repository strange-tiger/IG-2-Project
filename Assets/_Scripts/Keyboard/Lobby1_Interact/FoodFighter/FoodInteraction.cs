using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodInteraction : MonoBehaviour
{

    public UnityEvent OnActivateSatietyUI = new UnityEvent();
    public UnityEvent OnChangeSatietyUI = new UnityEvent();
    public UnityEvent OnDeactivateSatietyUI = new UnityEvent();
    public int SatietyStack { get; private set; }

    private AudioSource _audioSource;
    private Vector3 _initPosition;
    private Vector3 _nullPosition = new Vector3(0,0,0);
    private float _moveSpeed = 0.01f;
    private float _speedSlower = 0.0001f;
    private float _fatterCharacter = 0.1f;
    private float _walkCount;
    private int _dietWalkCount = 20;
    private int _maxSatietyStack = 6;

    private void OnEnable()
    {
        Food.OnEated.RemoveListener(EatFood);
        Food.OnEated.AddListener(EatFood);
    }


    void Update()
    {
        if (SatietyStack != 0)
        {
            Diet();
        }
    }

  

    void Diet()
    {
        if (_initPosition == _nullPosition)
            _initPosition = transform.position;
        

        if (Vector3.Distance(_initPosition, transform.position) >= 1)
        {
            _walkCount++;
            _initPosition = transform.position;
        }

        if (_walkCount == _dietWalkCount)
        {
            SatietyStack--;
            _walkCount = 0;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - _fatterCharacter);
            _moveSpeed += _speedSlower;


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


    public void EatFood(EFoodSatietyLevel foodSatietyLevel)
    {
        if(SatietyStack < 6)
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + (_fatterCharacter * (int)foodSatietyLevel));

            _moveSpeed -= _speedSlower * (int)foodSatietyLevel;


            if (SatietyStack == 0)
            {
                OnActivateSatietyUI.Invoke();


            }

            SatietyStack += (int)foodSatietyLevel;

            OnChangeSatietyUI.Invoke();
        }

    }


    private void OnDisable()
    {
        Food.OnEated.RemoveListener(EatFood);
    }

}

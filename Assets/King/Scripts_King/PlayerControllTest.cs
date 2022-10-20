using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerControllTest : MonoBehaviour
{

    [SerializeField] Image _stomachImage;
    [SerializeField] Image _currentSatietyStackImage;
    [SerializeField] Sprite[] _satietyStackImage;
    
    private Vector3 _initPosition;
    private float _moveSpeed = 0.01f;
    private float _interactDiastance = 5f;
    private float _fatterCharacter = 0.1f;
    private float _walkCount;
    private int _interativeLayer = 1 << 10;
    private int _satietyStack;
    private int _dietWalkCount = 20;

    void Start()
    {
        _stomachImage.gameObject.SetActive(false);
        _currentSatietyStackImage.gameObject.SetActive(false);
    }

    void Update()
    {
        TestMove();
        TestRayCast();
    }

    void TestMove()
    {
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical"));

        transform.position += direction * _moveSpeed;
        if(_satietyStack != 0)
        {
            Diet();
        }
        Debug.Log(_satietyStack);

    }

    void Diet()
    {
        if (_initPosition == null)
            _initPosition = transform.position;
        

        if (Vector3.Distance(_initPosition, transform.position) >= 1)
        {
            _walkCount++;
            _initPosition = transform.position;
        }

        if (_walkCount == _dietWalkCount)
        {
            _satietyStack--;
            Food.SatietyStack--;
            _walkCount = 0;

            if (_satietyStack != 0)
            {
                _currentSatietyStackImage.sprite = _satietyStackImage[_satietyStack - 1];
            }
            else
            {
                _currentSatietyStackImage.gameObject.SetActive(false);
                _stomachImage.gameObject.SetActive(false);
                _currentSatietyStackImage.sprite = null;
            }
        }
    }

    void TestRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin,ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, _interactDiastance, _interativeLayer))
        {
            if(hit.collider.CompareTag("Food") && Input.GetKeyDown(KeyCode.E))
            {
                if(_satietyStack < 6)
                {
                    hit.collider.GetComponentInParent<Food>().Eated();

                    EatFood();

                    if(_satietyStack > 6)
                    {
                        _satietyStack = 6;
                    }
                }
                
            }
        }
    }

    void EatFood()
    {


        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + _fatterCharacter);

        _moveSpeed -= 0.00001f;

        if(_satietyStack == 0)
        {
            _currentSatietyStackImage.gameObject.SetActive(true);

            _stomachImage.gameObject.SetActive(true);
        }
        _satietyStack = Food.SatietyStack;

        _currentSatietyStackImage.sprite = _satietyStackImage[_satietyStack - 1];
        
    }

   
}

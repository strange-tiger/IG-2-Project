using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FoodInteration : MonoBehaviour
{
    public static int SatietyStack { private get; set; }

    [SerializeField] Image _stomachImage;
    [SerializeField] Image _currentSatietyStackImage;
    [SerializeField] Sprite[] _satietyStackImage;
    private AudioSource _audioSource;
    private Vector3 _initPosition;
    private Vector3 _nullPosition = new Vector3(0,0,0);
    private float _moveSpeed = 0.01f;
    private float _speedSlower = 0.0001f;
    private float _interactDiastance = 5f;
    private float _fatterCharacter = 0.1f;
    private float _walkCount;
    private int _dietWalkCount = 20;
    private int _maxSatietyStack = 6;
    private int _interativeLayer = 1 << 10;

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
        if(SatietyStack != 0)
        {
            Diet();
        }
        Debug.Log(SatietyStack);

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

            if (SatietyStack == 0)
            {
                _currentSatietyStackImage.gameObject.SetActive(false);
                _stomachImage.gameObject.SetActive(false);
                _currentSatietyStackImage.sprite = null;

                _initPosition = _nullPosition;
            }
            else
            {
                _currentSatietyStackImage.sprite = _satietyStackImage[SatietyStack - 1];
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
                if(SatietyStack < _maxSatietyStack)
                {
                    hit.collider.GetComponentInParent<Food>().Eated();

                    EatFood();

                    if(SatietyStack > _maxSatietyStack)
                    {
                        SatietyStack = _maxSatietyStack;
                    }
                }
                
            }
        }
    }

    void EatFood()
    {


        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + _fatterCharacter);

        _moveSpeed -= _speedSlower;

        if(SatietyStack == 0)
        {
            _currentSatietyStackImage.gameObject.SetActive(true);

            _stomachImage.gameObject.SetActive(true);
        }

        _currentSatietyStackImage.sprite = _satietyStackImage[SatietyStack - 1];
        
    }

   
}

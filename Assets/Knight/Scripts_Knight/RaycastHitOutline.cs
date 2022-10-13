using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastHitOutline : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    public UnityEvent Hit = new UnityEvent();

    private float _rayLength = 5.0f;
    private int _object = 1 << 10;



    void Update()
    {
        if (_playerInput.IsRay)
        {
            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, _rayLength, _object))
            {
                Hit.Invoke();
            }

        }
    }
}

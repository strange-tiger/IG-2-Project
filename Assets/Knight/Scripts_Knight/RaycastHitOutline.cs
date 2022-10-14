using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastHitOutline : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    public UnityEvent OnInteractObject = new UnityEvent();
    public UnityEvent OutInteractObject = new UnityEvent();

    private float _rayLength = 5.0f;

    private int _layerMask = 1 << 10;

    private void Update()
    {
        if (_playerInput.IsRay)
        {
            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, _rayLength);

            if (hit.collider.gameObject.layer == _layerMask)
            {
                OnInteractObject.Invoke();
            }
            else
            {
                OutInteractObject.Invoke();
            }
        }
    }
}

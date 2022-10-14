using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastHitRightOutline : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    public UnityEvent OnRightInteractObject = new UnityEvent();
    public UnityEvent OutRightInteractObject = new UnityEvent();

    private Color _OutlineColor = new Color(42f / 255f, 244f / 255f, 37f / 255f);

    private float _rayLength = 5.0f;

    private int _layerMask = 1 << 10;

    private void Awake()
    {
        gameObject.AddComponent<Outlinable>();

        Outlinable _outlinable;

        _outlinable = GetComponent<Outlinable>();
        _outlinable.AddAllChildRenderersToRenderingList();
        _outlinable.OutlineParameters.Color = _OutlineColor;
    }

    private void Update()
    {
        if (_playerInput.IsLeftRay)
        {
            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, _rayLength);

            if (hit.collider.gameObject.layer == _layerMask)
            {
                OnRightInteractObject.Invoke();
            }
            else
            {
                OutRightInteractObject.Invoke();
            }


        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using System.Net.NetworkInformation;

public class PlayerFocus : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private bool isLeft;

    private LineRenderer _lineRenderer;

    private Vector3[] _rayPositions = new Vector3[2];
    [SerializeField] private Color _RayColor = new Color(42f / 255f, 244f / 255f, 37f / 255f);

    private float _rayLength = 5.0f;
    private float _alpha = 1.0f;

    private FocusableObjects _focusedObject;
    public FocusableObjects FocusedObject
    {
        get => _focusedObject;
        private set
        {
            if (value != _focusedObject)
            {
                if(_focusedObject)
                {
                    _focusedObject.OutFocus();
                }
                _focusedObject = value;
                _focusedObject.OnFocus();

                IsFocusedObjectChanged = true;
            }
            else
            {
                IsFocusedObjectChanged = false;
            }
        }
    }
    public bool IsFocusedObjectChanged { get; private set; }

    private LayerMask _offLayer;

    private void Awake()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();

        SetRayColor();

        _lineRenderer.enabled = false;

        _offLayer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        OnTriggerButton();
    }

    private void OnTriggerButton()
    {
        bool isRay = isLeft ? _playerInput.IsLeftRay : _playerInput.IsRightRay;

        if (isRay)
        {
            SetRayPosition();

            _lineRenderer.enabled = true;
        }
        else
        {
            FocusingNothing();

            _lineRenderer.enabled = false;
        }
    }

    private void SetRayPosition()
    {
        _rayPositions[0] = transform.position;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out hit, _rayLength))
        {
            _rayPositions[1] = hit.point;

            FocusableObjects focusObject = hit.collider.gameObject.GetComponent<FocusableObjects>();
            if(focusObject)
            {
                FocusedObject = focusObject;
            }
        }
        else
        {
            FocusingNothing();
        }

        _lineRenderer.positionCount = _rayPositions.Length;
        _lineRenderer.SetPositions(_rayPositions);
    }

    private void FocusingNothing()
    {
        if (FocusedObject)
        {
            FocusedObject.OutFocus();
            _focusedObject = null;
        }
        _rayPositions[1] = transform.position + transform.forward * _rayLength;
    }

    private void SetRayColor()
    {
        Gradient RayMaterialGradient = new Gradient();

        RayMaterialGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_RayColor, 0.0f), new GradientColorKey(_RayColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(_alpha, 0.0f), new GradientAlphaKey(0.0f, _alpha) }
            );
        _lineRenderer.colorGradient = RayMaterialGradient;
    }
}

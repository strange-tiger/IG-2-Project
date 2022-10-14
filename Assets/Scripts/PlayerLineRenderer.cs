using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using System.Net.NetworkInformation;

public class PlayerLineRenderer : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private LineRenderer _lineRenderer;

    private Vector3[] _rayPositions = new Vector3[2];
    private Color _RayColor = new Color(42f / 255f, 244f / 255f, 37f / 255f);

    private float _rayLength = 5.0f;
    private float _alpha = 1.0f;

    void Awake()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();

        SetRayColor();

        _lineRenderer.enabled = false;
    }

    void Update()
    {
        OnTriggerButton();
    }

    private void OnTriggerButton()
    {
        if (_playerInput.IsRay)
        {
            SetRayPosition();

            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    private void SetRayPosition()
    {
        _rayPositions[0] = transform.position;
        _rayPositions[1] = transform.position + transform.forward * _rayLength;

        _lineRenderer.positionCount = _rayPositions.Length;
        _lineRenderer.SetPositions(_rayPositions);
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

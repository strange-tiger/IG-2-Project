using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private LineRenderer _lineRenderer;

    private Vector3[] _rayPositions = new Vector3[2];
    private Color _startRayColor = new Vector4(42f, 244f, 37f);
    private Color _endRayColor = new Vector4(42f, 244f, 37f);

    private float _rayLength = 5.0f;
    private float alpha = 1.0f;

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
        if (_playerInput.isRay)
        {
            _lineRenderer.enabled = true;

            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, 10f);

        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }

    private void SetRayColor()
    {
        _lineRenderer.material = new Material(Shader.Find("RayColor"));

        _rayPositions[0] = transform.position;
        _rayPositions[1] = transform.position + transform.forward * _rayLength;

        _lineRenderer.positionCount = _rayPositions.Length;
        _lineRenderer.SetPositions(_rayPositions);

        Gradient RayMaterialGradient = new Gradient();

        RayMaterialGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_startRayColor, 0.0f), new GradientColorKey(_endRayColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(0.0f, 0.0f) }
            );
        _lineRenderer.colorGradient = RayMaterialGradient;
    }
}

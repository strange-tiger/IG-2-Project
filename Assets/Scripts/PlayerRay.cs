using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    private LineRenderer _lineRenderer;

    void Awake()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();

        _lineRenderer.positionCount = 2;
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
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, new Vector3(0f, 0f, 5f));

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
}

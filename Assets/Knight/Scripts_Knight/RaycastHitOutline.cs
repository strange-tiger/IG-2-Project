using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHitOutline : SensingObject
{
    [SerializeField]
    private PlayerInput _playerInput;

    private float _rayLength = 5.0f;

    private void Update()
    {
        if (_playerInput.IsRay)
        {
            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, _rayLength))
            {
                base.Awake();
            }
        }
    }

    public override void OnFocus()
    {
        base.OnFocus();
    }

    public override void OutFocus()
    {
        base.OutFocus();
    }

}

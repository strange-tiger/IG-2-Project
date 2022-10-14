using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : SensingObject
{
    [SerializeField]
    private RaycastHitLeftOutline _raycastHitOutline;

    private void Start()
    {
        _raycastHitOutline.OnInteractObject.RemoveListener(OnFocus);
        _raycastHitOutline.OnInteractObject.AddListener(OnFocus);

        _raycastHitOutline.OutInteractObject.RemoveListener(OutFocus);
        _raycastHitOutline.OutInteractObject.AddListener(OutFocus);
    }

    protected override void Awake()
    {
        base.Awake();
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

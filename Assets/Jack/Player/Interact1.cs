using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact1 : SensingObject1
{
    [SerializeField]
    private RaycastHitLeftOutline1 _raycastHitLeftOutline;

    private void Start()
    {

        _raycastHitLeftOutline.OnLeftInteractObject.RemoveListener(OnFocus);
        _raycastHitLeftOutline.OnLeftInteractObject.AddListener(OnFocus);

        _raycastHitLeftOutline.OutLeftInteractObject.RemoveListener(OutFocus);
        _raycastHitLeftOutline.OutLeftInteractObject.AddListener(OutFocus);
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

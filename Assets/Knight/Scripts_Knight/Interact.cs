using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : SensingObject
{
    [SerializeField]
    private RaycastHitLeftOutline _raycastHitLeftOutline;

    [SerializeField]
    private RaycastHitRightOutline _raycastHitRightOutline;

    private void Start()
    {
        _raycastHitLeftOutline.OnLeftInteractObject.RemoveListener(OnFocus);
        _raycastHitLeftOutline.OnLeftInteractObject.AddListener(OnFocus);

        _raycastHitLeftOutline.OutLeftInteractObject.RemoveListener(OutFocus);
        _raycastHitLeftOutline.OutLeftInteractObject.AddListener(OutFocus);

        _raycastHitRightOutline.OnRightInteractObject.RemoveListener(OnFocus);
        _raycastHitRightOutline.OnRightInteractObject.AddListener(OnFocus);

        _raycastHitRightOutline.OutRightInteractObject.RemoveListener(OutFocus);
        _raycastHitRightOutline.OutRightInteractObject.AddListener(OutFocus);
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

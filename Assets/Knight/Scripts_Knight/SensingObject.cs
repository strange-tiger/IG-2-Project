using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class SensingObject : MonoBehaviour
{
    private Outlinable _outlinable;
    private Color _OutlineColor = new Color(42f / 255f, 244f / 255f, 37f / 255f);

    protected virtual void Awake()
    {
        Reset(gameObject);
    }

    private void Start()
    {
        _outlinable = GetComponent<Outlinable>();
    }

    protected void Reset(GameObject target)
    {
        gameObject.layer = LayerMask.NameToLayer("Object");

        AddOutline(target);
    }

    private void AddOutline(GameObject target)
    {
        _outlinable = target.AddComponent<Outlinable>();
        if (_outlinable == null)
        {
            _outlinable = target.GetComponent<Outlinable>();
        }

        _outlinable.AddAllChildRenderersToRenderingList();
        _outlinable.OutlineParameters.Color = _OutlineColor;
        //_outlinable.FrontParameters.Color = _OutlineColor;

        OutFocus();
    }

    public virtual void OnFocus()
    {
        _outlinable.enabled = true;
    }

    public virtual void OutFocus()
    {
        _outlinable.enabled = false;
    }


}

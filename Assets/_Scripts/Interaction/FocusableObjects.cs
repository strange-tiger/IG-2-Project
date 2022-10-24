using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class FocusableObjects : MonoBehaviour
{
    protected Outlinable _outlineable;

    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);

    protected void Awake()
    {
        SetOutline();
        _outlineable.enabled = false;
    }

    protected void SetOutline()
    {
        _outlineable = GetComponent<Outlinable>();
        if (!_outlineable)
        {
            _outlineable = gameObject.AddComponent<Outlinable>();
        }

        _outlineable.AddAllChildRenderersToRenderingList();
        _outlineable.OutlineParameters.Color = _outlineColor;
    }

    public void OnFocus()
    {
        Debug.Log(gameObject.name + ": on");
        _outlineable.enabled = true;
    }

    public void OutFocus()
    {
        Debug.Log(gameObject.name + ": off");
        _outlineable.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using Photon.Pun;

public class FocusableObjects : MonoBehaviourPun
{
    protected Outlinable _outlineable;
    protected FocusableObjectsSencer _sencer;

    [SerializeField] private Color _sencedoutlineColor = new Color(0.576f, 0.745f, 1f);
    [SerializeField] private Color _onFocusOutlineColor = new Color(0.408f, 1f, 0f);
    [SerializeField] private float _outlineDilate = 1f;

    [SerializeField] private float _sencerRadius = 3.75f;


    protected void Awake()
    {
        SetOutline();
        _outlineable.enabled = false;
        SetSencer();
    }

    protected void OnEnable()
    {
        _outlineable.enabled = true;
    }

    protected void SetOutline()
    {
        _outlineable = GetComponent<Outlinable>();
        if (!_outlineable)
        {
            _outlineable = gameObject.AddComponent<Outlinable>();
        }

        _outlineable.AddAllChildRenderersToRenderingList();
        _outlineable.OutlineParameters.Color = _sencedoutlineColor;
        _outlineable.OutlineParameters.DilateShift = _outlineDilate;
    }

    protected void SetSencer()
    {
        _sencer = GetComponent<FocusableObjectsSencer>();
        if(!_sencer)
        {
            _sencer = gameObject.AddComponent<FocusableObjectsSencer>();
        }

        _sencer.SetSencer(_sencerRadius);
    }

    public void OnFocus()
    {
        Debug.Log(gameObject.name + ": on");
        _outlineable.OutlineParameters.Color = _onFocusOutlineColor;
    }

    public void OutFocus()
    {
        Debug.Log(gameObject.name + ": off");
        _outlineable.OutlineParameters.Color = _sencedoutlineColor;
    }

    protected void OnDisable()
    {
        _outlineable.enabled = false;
    }
}

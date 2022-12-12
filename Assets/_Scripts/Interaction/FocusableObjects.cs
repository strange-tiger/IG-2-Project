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

    [SerializeField] private bool _doNotNeedSencer = false;
    [SerializeField] private float _sencerRadius = 3.75f;


    protected virtual void Awake()
    {
        SetOutline();
        _outlineable.enabled = false;
        if(!_doNotNeedSencer)
        {
            SetSencer();
        }
    }

    protected virtual void OnEnable()
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
        _sencer = GetComponentInChildren<FocusableObjectsSencer>();
        if(!_sencer)
        {
            GameObject sencerObejct = new GameObject();
            sencerObejct.transform.parent = transform;
            sencerObejct.transform.position = transform.position;
            _sencer = sencerObejct.AddComponent<FocusableObjectsSencer>();
        }


        SyncOVRGrabbable _grabbable = GetComponent<SyncOVRGrabbable>();
        if (_grabbable)
        {
            _grabbable.CallbackOnGrabBegin.RemoveListener(OnGrabBegin);
            _grabbable.CallbackOnGrabBegin.AddListener(OnGrabBegin);

            _grabbable.CallbackOnGrabEnd.RemoveListener(OnGrabEnd);
            _grabbable.CallbackOnGrabEnd.AddListener(OnGrabEnd);
        }

        _sencer.SetSencer(_sencerRadius, this);
    }

    private void OnGrabBegin()
    {
        _sencer.gameObject.SetActive(false);
    }

    private void OnGrabEnd()
    {
        _sencer.gameObject.SetActive(true);
    }

    public virtual void OnFocus()
    {
        Debug.Log(gameObject.name + ": on");
        _outlineable.OutlineParameters.Color = _onFocusOutlineColor;
    }

    public virtual void OutFocus()
    {
        Debug.Log(gameObject.name + ": off");
        _outlineable.OutlineParameters.Color = _sencedoutlineColor;
    }

    protected virtual void OnDisable()
    {
        _outlineable.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class GoldBoxSencer : MonoBehaviour
{
    [SerializeField] private Color _outlineColor = new Color(1f, 0.9f, 0.01f);
    private Outlinable _outline;

    private GameObject _goldBoxInteractionObject;

    private void Awake()
    {
        _goldBoxInteractionObject = GetComponentInChildren<GoldBoxInetraction>().gameObject;

        _outline = GetComponent<Outlinable>();
        _outline.AddAllChildRenderersToRenderingList();
        _outline.OutlineParameters.Color = _outlineColor;
    }
}

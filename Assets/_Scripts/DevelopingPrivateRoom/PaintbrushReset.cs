using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintbrushReset : MonoBehaviour
{
    [SerializeField] Button _resetButton;

    public event Action OnReset;

    private void OnEnable()
    {
        _resetButton.onClick.RemoveListener(ResetDraw);
        _resetButton.onClick.AddListener(ResetDraw);
    }

    private void OnDisable()
    {
        _resetButton.onClick.RemoveListener(ResetDraw);
    }

    private void ResetDraw()
    {
        for(int i = 1; i < transform.childCount; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        OnReset.Invoke();
    }
}

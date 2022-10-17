using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    void Start()
    {
        Invoke("SettingPanal", 1.2f);
    }

    private void SettingPanal()
    {
        _canvas.SetActive(false);
    }
}

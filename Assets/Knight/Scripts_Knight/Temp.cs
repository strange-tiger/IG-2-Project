using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas;

    void Start()
    {
        _canvas.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInUIManager : MonoBehaviour
{
    public enum EWindow
    {
        LOGIN,
        SIGNIN,
        FINDPASSWORD,
        MAX
    }

    [SerializeField] GameObject[] Windows;

}

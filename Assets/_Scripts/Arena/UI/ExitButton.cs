using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    private GameObject _moveArenaPanel;

    public void OnClickNoButton()
    {
        _moveArenaPanel.SetActive(false);
    }
}

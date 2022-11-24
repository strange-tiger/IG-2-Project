using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomUI : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;

    void Start()
    {
        _menuPanel.SetActive(false);
    }
}

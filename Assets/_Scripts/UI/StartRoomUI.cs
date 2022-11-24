using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoomUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuPanels;

    void Start()
    {
        foreach(GameObject panel in _menuPanels)
        {
            panel.SetActive(false);
        }
    }
}

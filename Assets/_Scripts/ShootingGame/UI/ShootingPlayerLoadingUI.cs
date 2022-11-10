using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlayerLoadingUI : MonoBehaviour
{
    [SerializeField] private GameObject _loadingPanel;

    public void DisableLoadingPanel()
    {
        _loadingPanel.SetActive(false);
    }
}

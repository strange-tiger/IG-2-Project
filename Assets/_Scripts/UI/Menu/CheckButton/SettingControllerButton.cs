using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingControllerButton : MonoBehaviour
{
    [SerializeField] private GameObject _soundPanle;
    [SerializeField] private GameObject _controllerPanle;
    public void OnClickSoundButton()
    {
        _soundPanle.SetActive(true);
        _controllerPanle.SetActive(false);
    }

    public void OnClickControllerButton()
    {
        _soundPanle.SetActive(false);
        _controllerPanle.SetActive(true);
    }
}

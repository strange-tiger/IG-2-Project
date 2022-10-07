using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField]
    private GameObject _rayBox;

    void Start()
    {
        _rayBox.SetActive(false);
    }

    void Update()
    {
        OnTriggerButton();
    }

    private void OnTriggerButton()
    {
        if (_playerInput.isRay)
        {
            _rayBox.SetActive(true);

            Ray ray;
            RaycastHit hit;

            ray = new Ray(transform.position, transform.forward);
            Physics.Raycast(ray, out hit, 10f);

        }
        else
        {
            _rayBox.SetActive(false);
        }

    }
}

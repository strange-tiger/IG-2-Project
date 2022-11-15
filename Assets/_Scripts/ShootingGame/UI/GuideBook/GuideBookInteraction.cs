using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideBookInteraction : InteracterableObject
{
    [SerializeField] private GuideBookUIManager _guideBook;
    private Transform _guideBookTransform;
    private Transform _playerTransform;

    private bool _isInteracted;

    private void Start()
    {
        _guideBook.OnClose.RemoveAllListeners();
        _guideBook.OnClose.AddListener(InteractionOver);
        _guideBookTransform = _guideBook.transform;
    }

    public override void Interact()
    {
        _isInteracted = true;
        _guideBook.gameObject.SetActive(true);
        _playerTransform = MenuUIManager.Instance.transform;
    }

    private void InteractionOver()
    {
        _isInteracted = false;
    }

    private void Update()
    {
        if(_isInteracted)
        {
            _guideBookTransform.LookAt(_playerTransform);
        }
    }
}

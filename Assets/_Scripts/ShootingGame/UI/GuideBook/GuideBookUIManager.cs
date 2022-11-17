using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GuideBookUIManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip _bookOpenAndCloseClip;
    [SerializeField] private AudioClip _pageFlipClip;

    [Header("Pages")]
    [SerializeField] GameObject[] _guildPages;
    [SerializeField] GameObject _leftPageUI;
    [SerializeField] GameObject _rightPageUI;
    private int _currentPage;

    public UnityEvent OnClose = new UnityEvent();

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _audioSource.PlayOneShot(_bookOpenAndCloseClip);
        _guildPages[_currentPage].SetActive(false);
        _guildPages[0].SetActive(true);
        _currentPage = 0;
    }

    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            PageUp();
        }
        else if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            PageDown();
        }
        else if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            _audioSource.PlayOneShot(_bookOpenAndCloseClip);
            gameObject.SetActive(false);
            OnClose.Invoke();
        }
    }

    private void PageUp()
    {
        if(_currentPage >= _guildPages.Length - 1)
        {
            return;
        }

        _audioSource.PlayOneShot(_pageFlipClip);
        _guildPages[_currentPage].gameObject.SetActive(false);
        ++_currentPage;
        _guildPages[_currentPage].gameObject.SetActive(true);

        _leftPageUI.gameObject.SetActive(true);
        _rightPageUI.gameObject.SetActive(_currentPage < _guildPages.Length - 1);
    }

    private void PageDown()
    {
        if(_currentPage <= 0)
        {
            return;
        }

        _audioSource.PlayOneShot(_pageFlipClip);
        _guildPages[_currentPage].gameObject.SetActive(false);
        --_currentPage;
        _guildPages[_currentPage].gameObject.SetActive(true);

        _leftPageUI.gameObject.SetActive(_currentPage > 0);
        _rightPageUI.gameObject.SetActive(true);
    }
}
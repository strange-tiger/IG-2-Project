using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] GameObject _radialMenu;
    [SerializeField] Button _upButton;
    [SerializeField] Button _downButton;
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _defaultButton;
    [SerializeField] Image _feelingImage;
    [SerializeField] Image _upButtonImage;
    [SerializeField] Image _downButtonImage;
    [SerializeField] Image _leftButtonImage;
    [SerializeField] Image _rightButtonImage;
    [SerializeField] Image _defaultButtonImage;

    private static readonly YieldInstruction _waitSecond = new WaitForSeconds(3f);
    private static Color _activeColor = new Color(1, 1, 1, 1);
    private static Color _deactiveColor = new Color(0,0,0,0);
    private void OnEnable()
    {
        _upButton.onClick.RemoveListener(UpMenu);
        _upButton.onClick.AddListener(UpMenu);

        _downButton.onClick.RemoveListener(DownMenu);
        _downButton.onClick.AddListener(DownMenu);

        _leftButton.onClick.RemoveListener(LeftMenu);
        _leftButton.onClick.AddListener(LeftMenu);

        _rightButton.onClick.RemoveListener(RightMenu);
        _rightButton.onClick.AddListener(RightMenu);

        _defaultButton.onClick.RemoveListener(DefaultMenu);
        _defaultButton.onClick.AddListener(DefaultMenu);
    }

   
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            _radialMenu.SetActive(true);
        }
    }
    IEnumerator OffFeelingImage()
    {

        yield return _waitSecond;

        _feelingImage.color = _deactiveColor;

        yield return null;
    }

    private void UpMenu()
    {
        StopCoroutine(OffFeelingImage());
        _feelingImage.color = _activeColor;
        _feelingImage.sprite = _upButtonImage.sprite;
        _radialMenu.SetActive(false);
        StartCoroutine(OffFeelingImage());
    }

    private void DownMenu()
    {
        StopCoroutine(OffFeelingImage());
        _feelingImage.color = _activeColor;
        _feelingImage.sprite = _downButtonImage.sprite;
        _radialMenu.SetActive(false);
        StartCoroutine(OffFeelingImage());
    }

    private void LeftMenu()
    {
        StopCoroutine(OffFeelingImage());
        _feelingImage.color = _activeColor;
        _feelingImage.sprite = _leftButtonImage.sprite;
        _radialMenu.SetActive(false);
        StartCoroutine(OffFeelingImage());
    }

    private void RightMenu()
    {
        StopCoroutine(OffFeelingImage());
        _feelingImage.color = _activeColor;
        _feelingImage.sprite = _rightButtonImage.sprite;
        _radialMenu.SetActive(false);
        StartCoroutine(OffFeelingImage());
    }

    private void DefaultMenu()
    {
        StopCoroutine(OffFeelingImage());
        _feelingImage.color = _activeColor;
        _feelingImage.sprite = _defaultButtonImage.sprite;
        _radialMenu.SetActive(false);
        StartCoroutine(OffFeelingImage());
    }

    private void OnDisable()
    {
        _upButton.onClick.RemoveListener(UpMenu);
        _downButton.onClick.RemoveListener(DownMenu);
        _leftButton.onClick.RemoveListener(LeftMenu);
        _rightButton.onClick.RemoveListener(RightMenu);
        _defaultButton.onClick.RemoveListener(DefaultMenu);

    }
}

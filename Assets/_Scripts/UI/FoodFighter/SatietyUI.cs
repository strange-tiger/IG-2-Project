using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SatietyUI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private FoodInteraction _foodInteraction;

    [Header("UI Image")]
    [SerializeField] private Image _stomachImage;
    [SerializeField] private Image _currentSatietyStackImage;
    [SerializeField] private Sprite[] _satietyStackImage;

    private void Awake()
    {
        _foodInteraction = transform.root.GetComponent<FoodInteraction>();
    }

    private void OnEnable()
    {
        _foodInteraction.OnActivateSatietyUI.RemoveListener(ActivateUI);
        _foodInteraction.OnActivateSatietyUI.AddListener(ActivateUI);

        _foodInteraction.OnChangeSatietyUI.RemoveListener(ChangeUI);
        _foodInteraction.OnChangeSatietyUI.AddListener(ChangeUI);

        _foodInteraction.OnDeactivateSatietyUI.RemoveListener(DeactivateUI);
        _foodInteraction.OnDeactivateSatietyUI.AddListener(DeactivateUI);
    }

    private void ActivateUI()
    {
        _currentSatietyStackImage.gameObject.SetActive(true);

        _stomachImage.gameObject.SetActive(true);
    }

    private void ChangeUI()
    {
        _currentSatietyStackImage.sprite = _satietyStackImage[_foodInteraction.SatietyStack - 1];
    }

    private void DeactivateUI()
    {
        _currentSatietyStackImage.gameObject.SetActive(false);

        _stomachImage.gameObject.SetActive(false);

        _currentSatietyStackImage.sprite = null;
    }

    private void OnDisable()
    {
        _foodInteraction.OnActivateSatietyUI.RemoveListener(ActivateUI);
        _foodInteraction.OnChangeSatietyUI.RemoveListener(ChangeUI);
        _foodInteraction.OnDeactivateSatietyUI.RemoveListener(DeactivateUI);
    }
}

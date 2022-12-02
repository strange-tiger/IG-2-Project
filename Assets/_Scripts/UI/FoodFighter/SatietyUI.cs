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

    /// <summary>
    /// FoodInteraction의 이벤트를 찾아옴.
    /// </summary>
    private void OnEnable()
    {
        _foodInteraction.OnActivateSatietyUI.RemoveListener(ActivateUI);
        _foodInteraction.OnActivateSatietyUI.AddListener(ActivateUI);

        _foodInteraction.OnChangeSatietyUI.RemoveListener(ChangeUI);
        _foodInteraction.OnChangeSatietyUI.AddListener(ChangeUI);

        _foodInteraction.OnDeactivateSatietyUI.RemoveListener(DeactivateUI);
        _foodInteraction.OnDeactivateSatietyUI.AddListener(DeactivateUI);
    }

    /// <summary>
    /// 포만감 스택이 쌓이면 포만감 UI를 활성화 시킴.
    /// </summary>
    private void ActivateUI()
    {
        _currentSatietyStackImage.gameObject.SetActive(true);

        _stomachImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 포만감 스택이 변화할 때, 그에 맞는 이미지를 바꿔줌. 
    /// </summary>
    private void ChangeUI()
    {
        _currentSatietyStackImage.sprite = _satietyStackImage[_foodInteraction.SatietyStack - 1];
    }


    /// <summary>
    /// 포만감이 사라지면, UI를 비활성화 시킴.
    /// </summary>
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

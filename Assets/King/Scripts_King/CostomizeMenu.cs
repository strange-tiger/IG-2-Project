using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostomizeMenu : MonoBehaviour
{

    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    public CostomizeData _costomizeDatas;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    int _setNum = 0;

    private void OnEnable()
    {
        _leftButton.onClick.RemoveListener(LeftButton);
        _leftButton.onClick.AddListener(LeftButton);

        _rightButton.onClick.RemoveListener(RightButton);
        _rightButton.onClick.AddListener(RightButton);
    }

    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setNum];
    }



    void LeftButton()
    {
        if(_setNum == 0)
        {
            _setNum = 2;
        }
        else
        {
            _setNum -= 1;
        }
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setNum];

    }

    void RightButton()
    {
        if (_setNum == 2)
        {
            _setNum = 0;
        }
        else
        {
            _setNum += 1;
        }
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setNum];

    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(LeftButton);
        _rightButton.onClick.RemoveListener(RightButton);
    }
}

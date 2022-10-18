using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostomizeMenu : MonoBehaviour
{

    [SerializeField] Button _leftAvatarButton;
    [SerializeField] Button _leftMaterialButton;
    [SerializeField] Button _rightAvatarButton;
    [SerializeField] Button _rightMaterialButton;
    [SerializeField] GameObject _noneLight;
    [SerializeField] Material _noneMaterial;
    public CostomizeData _costomizeDatas;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    int _setAvatarNum;
    int _setMaterialNum = 0;

    private void OnEnable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _leftAvatarButton.onClick.AddListener(LeftAvartarButton);

        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _rightAvatarButton.onClick.AddListener(RightAvatarButton);

        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _leftMaterialButton.onClick.AddListener(LeftMaterialButton);

        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
        _rightMaterialButton.onClick.AddListener(RightMaterialButton);
    }

    void Start()
    {
        _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        
        for(int i = 0; i < _costomizeDatas.AvatarState.Length - 1; ++i)
        {
            if(_costomizeDatas.AvatarState[i] == EAvartarState.EQUIED)
            {
                _setAvatarNum = i;
                break;
            }
        }
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];
    }



    void LeftAvartarButton()
    {
        if(_setAvatarNum == 0)
        {
            _setAvatarNum = _costomizeDatas.AvatarGameObject.Length - 1;
        }
        else
        {
            _setAvatarNum -= 1;
        }
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];
        
        if(_costomizeDatas.AvatarState[_setAvatarNum] == EAvartarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
    }

    void RightAvatarButton()
    {
        if (_setAvatarNum == _costomizeDatas.AvatarGameObject.Length - 1)
        {
            _setAvatarNum = 0;
        }
        else
        {
            _setAvatarNum += 1;
        }
        
        _skinnedMeshRenderer.sharedMesh = _costomizeDatas.AvatarGameObject[_setAvatarNum];

        if (_costomizeDatas.AvatarState[_setAvatarNum] == EAvartarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
            _noneLight.SetActive(false);
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            _noneLight.SetActive(true);
        }
    }

    void LeftMaterialButton()
    {
        if (_setMaterialNum == 0)
        {
            _setMaterialNum = _costomizeDatas.AvatarMaterial.Length - 1;
        }
        else
        {
            _setMaterialNum -= 1;
        }

         if (_costomizeDatas.AvatarState[_setAvatarNum] == EAvartarState.NONE)
            {
                _skinnedMeshRenderer.material = _noneMaterial;
            }
            else
            {
                _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
            }
    }
    void RightMaterialButton()
    {
        if (_setMaterialNum == _costomizeDatas.AvatarMaterial.Length - 1)
        {
            _setMaterialNum = 0;
        }
        else
        {
            _setMaterialNum += 1;
        }

        if (_costomizeDatas.AvatarState[_setAvatarNum] == EAvartarState.NONE)
        {
            _skinnedMeshRenderer.material = _noneMaterial;
        }
        else
        {
            _skinnedMeshRenderer.material = _costomizeDatas.AvatarMaterial[_setMaterialNum];
        }
    }

    private void OnDisable()
    {
        _leftAvatarButton.onClick.RemoveListener(LeftAvartarButton);
        _rightAvatarButton.onClick.RemoveListener(RightAvatarButton);
        _leftMaterialButton.onClick.RemoveListener(LeftMaterialButton);
        _rightMaterialButton.onClick.RemoveListener(RightMaterialButton);
    }
}

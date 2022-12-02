using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using System.Net.NetworkInformation;

public class PlayerFocus : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private bool isLeft;

    // LineRenderer 관련
    private LineRenderer _lineRenderer;
    private Vector3[] _rayPositions = new Vector3[2];
    [SerializeField] private Color _rayColor = new Color(42f / 255f, 244f / 255f, 37f / 255f);

    private float _rayLength = 3.0f;
    private float _alpha = 1.0f;

    // 조준한 오브젝트 관련
    private FocusableObjects _focusedObject;
    /// <summary>
    /// 조준한 오브젝트. null일 수 있음
    /// </summary>
    public FocusableObjects FocusedObject
    {
        get => _focusedObject;
        private set
        {
            // 만약 새로 들어온 오브젝트라면 기존의 오브젝트에 OnFocus 효과를 꺼줌
            if (value != _focusedObject)
            {
                if(HaveFocuseObject)
                {
                    _focusedObject.OutFocus();
                }
                _focusedObject = value;
                _focusedObject.OnFocus();

                HaveFocuseObject = true;
            }
        }
    }
    /// <summary>
    /// 조준한 오브젝트있는지 확인
    /// </summary>
    public bool HaveFocuseObject { get; private set; }

    private void Awake()
    {
        // 라인렌더러 설정
        _lineRenderer = GetComponentInChildren<LineRenderer>();
        SetRayColor();
        _lineRenderer.enabled = false;
    }

    private void Update()
    {
        if(PlayerControlManager.Instance.IsRayable)
        {
            OnTriggerButton();
        }
        else
        {
            FocusingNothing();

            _lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// 자신(오른손 or 왼손)의 레이 버튼이 눌렸다면 레이를 쏨
    /// </summary>
    private void OnTriggerButton()
    {
        bool isRay = isLeft ? _playerInput.IsLeftRay : _playerInput.IsRightRay;

        if (isRay)
        {
            SetRayPosition();

            _lineRenderer.enabled = true;
        }
        else
        {
            FocusingNothing();

            _lineRenderer.enabled = false;
        }
    }

    /// <summary>
    /// 레이를 쏘아, 닿은 상대가 조준 가능한 오브젝트(FocusableObjects)라면 해당 정보를 저장함
    /// </summary>
    private void SetRayPosition()
    {
        _rayPositions[0] = transform.position;

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if(Physics.Raycast(ray, out hit, _rayLength))
        {
            _rayPositions[1] = hit.point;

            FocusableObjects focusObject = hit.collider.gameObject.GetComponent<FocusableObjects>();
            if(focusObject)
            {
                FocusedObject = focusObject;
            }
            else
            {
                // 콜라이더 맞은 대상에게 FocusableObjects가 없다면 부모 중에 찾음
                focusObject = hit.collider.gameObject.GetComponentInParent<FocusableObjects>();
                if(focusObject)
                {
                    FocusedObject = focusObject;
                }
            }
        }
        else
        {
            FocusingNothing();
        }

        _lineRenderer.positionCount = _rayPositions.Length;
        _lineRenderer.SetPositions(_rayPositions);
    }

    private void FocusingNothing()
    {
        if (HaveFocuseObject)
        {
            FocusedObject.OutFocus();
            _focusedObject = null;
        }
        HaveFocuseObject = false;
        _rayPositions[1] = transform.position + transform.forward * _rayLength;
    }

    private void SetRayColor()
    {
        Gradient RayMaterialGradient = new Gradient();

        RayMaterialGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(_rayColor, 0.0f), new GradientColorKey(_rayColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(_alpha, 0.0f), new GradientAlphaKey(0.0f, _alpha) }
            );
        _lineRenderer.colorGradient = RayMaterialGradient;
    }
}

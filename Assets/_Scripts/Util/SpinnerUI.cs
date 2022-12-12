using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpinnerUI : MonoBehaviour
{
    [SerializeField] protected Button _leftButton;
    [SerializeField] protected Button _rightButton;
    [SerializeField] protected TextMeshProUGUI _currentStateText;

    [SerializeField] protected string[] _states;

    protected Dictionary<string, TypeDelegate> _stateFunctionTable = new Dictionary<string, TypeDelegate>();
    public virtual string Type
    {
        get
        {
            return _type;
        }
        protected set
        {
            _type = value;
            _currentStateText.text = _type;
            _stateFunctionTable[_type]?.Invoke();
        }
    }
    protected string _type;
    protected int _index;

    protected delegate void TypeDelegate();

    protected virtual void OnEnable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _leftButton.onClick.AddListener(OnClickLeftButton);

        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _rightButton.onClick.AddListener(OnClickRightButton);

        Type = _states[0];
        _stateFunctionTable.Clear();
    }

    protected virtual void OnDisable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _rightButton.onClick.RemoveListener(OnClickRightButton);
    }

    /// <summary>
    /// UI의 상태 인덱스를 하나 감소
    /// 감소된 인덱스가 0 이하라면 최대 인덱스로 변경
    /// </summary>
    public virtual void OnClickLeftButton()
    {
        if (_index - 1 < 0)
        {
            _index = _states.Length;
        }
        --_index;
        Type = _states[_index];
    }

    /// <summary>
    /// UI의 상태 인덱스를 하나 증가
    /// 증가된 인덱스가 최대 인덱스 이상이라면 0으로 변경
    /// </summary>
    public virtual void OnClickRightButton()
    {
        if (_index + 1 >= _states.Length)
        {
            _index = -1;
        }
        ++_index;
        Type = _states[_index];
    }
}

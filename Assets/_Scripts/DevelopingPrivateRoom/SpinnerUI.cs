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

    public virtual void OnClickLeftButton()
    {
        if (_index - 1 < 0)
        {
            _index = _states.Length;
        }
        --_index;
        Type = _states[_index];
    }

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

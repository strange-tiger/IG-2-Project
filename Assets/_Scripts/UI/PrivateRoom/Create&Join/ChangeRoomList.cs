using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRoomList : SpinnerUI
{
    [SerializeField] JoinRoomUI _ui;

    public override string Type
    {
        get
        {
            return _type;
        }
        protected set
        {
            _type = value;
            _currentStateText.text = _type;
        }
    }

    protected override void OnEnable()
    {
        _leftButton.onClick.RemoveListener(OnClickLeftButton);
        _leftButton.onClick.AddListener(OnClickLeftButton);

        _rightButton.onClick.RemoveListener(OnClickRightButton);
        _rightButton.onClick.AddListener(OnClickRightButton);

        JoinRoomUI.OnPageCountChanged -= UpdateStates;
        JoinRoomUI.OnPageCountChanged += UpdateStates;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        JoinRoomUI.OnPageCountChanged -= UpdateStates;
    }

    private void Start()
    {
        UpdateStates();
    }

    private int _pageCount = 0;
    public void UpdateStates()
    {
        if (JoinRoomUI.PageCount > 0)
        {
            _pageCount = JoinRoomUI.PageCount;
        }
        else
        {
            _pageCount = 1;
        }
        
        _states = new string[_pageCount];

        _stateFunctionTable.Clear();

        for (int i = 0; i < _pageCount; ++i)
        {
            _states[i] = $"{i + 1} / {_pageCount}";
        }

        Type = _states[0];
    }

    public override void OnClickLeftButton()
    {
        base .OnClickLeftButton();

        _ui.ChangeRoomPage(_index);
    }

    public override void OnClickRightButton()
    {
        base.OnClickRightButton();

        _ui.ChangeRoomPage(_index);
    }
}

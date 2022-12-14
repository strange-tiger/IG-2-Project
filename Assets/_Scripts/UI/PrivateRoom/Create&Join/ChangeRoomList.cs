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
    /// <summary>
    /// 룸 리스트의 페이지 수를 구한다. 
    /// _pageCount에 현재 페이지 수 JoinRoomUI.PageCount을 할당하고, 만약 0이라면 _pageCount = 1로 한다.
    /// _states의 각 상태 메시지를 "1 / 2" 같은 페이지 카운팅으로 할당한다.
    /// </summary>
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

    /// <summary>
    /// 변경된 인덱스 _index에 따라 _ui.ChangeRoomPage를 호출한다.
    /// </summary>
    public override void OnClickLeftButton()
    {
        base.OnClickLeftButton();

        _ui.ChangeRoomPage(_index);
    }

    /// <summary>
    /// 변경된 인덱스 _index에 따라 _ui.ChangeRoomPage를 호출한다.
    /// </summary>
    public override void OnClickRightButton()
    {
        base.OnClickRightButton();

        _ui.ChangeRoomPage(_index);
    }
}

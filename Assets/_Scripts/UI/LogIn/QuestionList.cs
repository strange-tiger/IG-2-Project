using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestionList : SpinnerUI
{
    public int Value { get => _index; }

    public override string Type 
    { 
        get => base.Type;
        protected set
        {
            _type = value;
            _currentStateText.text = _type;
        }
    }

    public override void OnClickLeftButton()
    {
        base.OnClickLeftButton();

        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnClickRightButton()
    {
        base.OnClickRightButton();

        EventSystem.current.SetSelectedGameObject(null);
    }
}

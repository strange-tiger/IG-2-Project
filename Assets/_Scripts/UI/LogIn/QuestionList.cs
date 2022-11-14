using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

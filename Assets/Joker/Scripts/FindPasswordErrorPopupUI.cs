using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Error = Defines.EErrorType;

public class FindPasswordErrorPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI _emailErrorMessage;
    [SerializeField] TextMeshProUGUI _answerErrorMessage;

    /// <summary>
    /// 에러 타입을 전하는 Defines.EErrorType error 매개변수를 받아
    /// 에러 타입에 맞는 텍스트를 활성화하면서 팝업(활성화)
    /// </summary>
    /// <param name="error"></param>
    public void ErrorPopup(Error error)
    {
        Debug.Assert(error != Error.NONE && error != Error.MAX, "Error in Error");
        gameObject.SetActive(true);
        
        if (error == Error.EMAIL)
        {
            _emailErrorMessage.gameObject.SetActive(true);
            _answerErrorMessage.gameObject.SetActive(false);
        }
        else if (error == Error.ANSWER)
        {
            _emailErrorMessage.gameObject.SetActive(false);
            _answerErrorMessage.gameObject.SetActive(true);
        }
    }
}

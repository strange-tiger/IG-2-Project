using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindPasswordErrorPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI _emailErrorMessage;
    [SerializeField] TextMeshProUGUI _answerErrorMessage;

    public void ErrorPopup(Defines.EErrorType error)
    {
        Debug.Assert(error != Defines.EErrorType.NONE && error != Defines.EErrorType.MAX, "Error in Error");
        gameObject.SetActive(true);
        
        if (error == Defines.EErrorType.EMAIL)
        {
            _emailErrorMessage.gameObject.SetActive(true);
            _answerErrorMessage.gameObject.SetActive(false);
        }
        else if (error == Defines.EErrorType.ANSWER)
        {
            _emailErrorMessage.gameObject.SetActive(false);
            _answerErrorMessage.gameObject.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Error = Defines.EFindPasswordErrorType;

public class FindPasswordErrorPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI _idErrorMessage;
    [SerializeField] TextMeshProUGUI _answerErrorMessage;

    public void ErrorPopup(Error error)
    {
        Debug.Assert(error != Error.NONE && error != Error.MAX, "Error in Error");
        gameObject.SetActive(true);
        
        if (error == Error.ID)
        {
            _idErrorMessage.gameObject.SetActive(true);
            _answerErrorMessage.gameObject.SetActive(false);
        }
        else if (error == Error.ANSWER)
        {
            _idErrorMessage.gameObject.SetActive(false);
            _answerErrorMessage.gameObject.SetActive(true);
        }
    }
}

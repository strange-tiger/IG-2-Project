using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using Error = Defines.ELogInErrorType;

public class LogInErrorPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI _idErrorMessage;
    [SerializeField] TextMeshProUGUI _passwordErrorMessage;
    [SerializeField] TextMeshProUGUI _duplicatedErrorMessage;

    public void ErrorPopup(Error error)
    {
        Debug.Assert(error != Error.NONE && error != Error.MAX, "Error in Error");
        gameObject.SetActive(true);

        if (error == Error.ID)
        {
            _idErrorMessage.gameObject.SetActive(true);
            _passwordErrorMessage.gameObject.SetActive(false);
            _duplicatedErrorMessage.gameObject.SetActive(false);
        }
        else if (error == Error.PASSWORD)
        {
            _idErrorMessage.gameObject.SetActive(false);
            _passwordErrorMessage.gameObject.SetActive(true);
            _duplicatedErrorMessage.gameObject.SetActive(false);
        }
        else if (error == Error.DUPLICATED)
        {
            _idErrorMessage.gameObject.SetActive(false);
            _passwordErrorMessage.gameObject.SetActive(false);
            _duplicatedErrorMessage.gameObject.SetActive(true);
        }
    }
}

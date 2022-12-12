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

    /// <summary>
    /// 로그인 에러의 타입 error에 따라 활성화할 텍스트 오브젝트를 결정
    /// </summary>
    /// <param name="error"></param>
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Error = Defines.EChangePasswordErrorType;

public class ChangePasswordErrorPopupUI : PopupUI
{
    [Header("Message")]
    [SerializeField] TextMeshProUGUI _idErrorMessage;
    [SerializeField] TextMeshProUGUI _answerErrorMessage;

    /// <summary>
    /// 비밀번호 변경을 할 수 없는 에러 발생 시, 이유를 알리기 위해 에러의 타입 error에 따라 활성화할 텍스트 오브젝트를 결정
    /// </summary>
    /// <param name="error"></param>
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

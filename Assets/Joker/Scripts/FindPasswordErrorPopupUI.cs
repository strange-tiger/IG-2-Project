using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Error = Defines.EFindPasswordErrorType;

public class FindPasswordErrorPopupUI : PopupUI
{
    [SerializeField] TextMeshProUGUI _emailErrorMessage;
    [SerializeField] TextMeshProUGUI _answerErrorMessage;

    /// <summary>
    /// ���� Ÿ���� ���ϴ� Defines.EErrorType error �Ű������� �޾�
    /// ���� Ÿ�Կ� �´� �ؽ�Ʈ�� Ȱ��ȭ�ϸ鼭 �˾�(Ȱ��ȭ)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    /// <summary>
    /// �α��� ���� UI �ε���
    /// </summary>
    public enum ELogInUIIndex
    {
        LOGIN,
        SIGNIN,
        FINDPASSWORD,
        MAX
    }

    public enum ELogInErrorType
    {
        NONE,
        ID,
        PASSWORD,
        MAX
    }

    public enum EFindPasswordErrorType
    {
        NONE,
        EMAIL,
        ANSWER,
        MAX
    }

    public enum ECharacterUIIndex
    {
        SELECT,
        MAKE,
        MAX
    }
}

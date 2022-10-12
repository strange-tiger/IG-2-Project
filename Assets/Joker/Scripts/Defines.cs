using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Defines
{
    /// <summary>
    /// 로그인 씬의 UI 인덱스
    /// </summary>
    public enum ELogInUIIndex
    {
        LOGIN,
        SIGNIN,
        FINDPASSWORD,
        MAX
    }

    /// <summary>
    /// FindPassword UI에서의 에러 타입 인덱스
    /// </summary>
    public enum EErrorType
    {
        NONE,
        EMAIL,
        ANSWER,
        MAX
    }
}

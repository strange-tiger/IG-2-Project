using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivateRoomRadialCursor : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 Button 태그의 UI 오브젝트와 트리거 충돌하면 
    /// PrivateRoomRadialMenu의 ClickButton에 충돌한 오브젝트의 Button을 할당하고
    /// Select 함수를 호출한다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            PrivateRoomRadialMenu.ClickButton = collision.GetComponent<Button>();
            PrivateRoomRadialMenu.ClickButton.Select();
        }
    }

    /// <summary>
    /// 이 오브젝트가 Button 태그의 UI 오브젝트와 더 이상 트리거 충돌하지 않으면 
    /// PrivateRoomRadialMenu의 ClickButton 할당을 취소한다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            PrivateRoomRadialMenu.ClickButton = null;
        }
    }
}

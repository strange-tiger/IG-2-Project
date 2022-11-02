using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivateRoomRadialCursor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            PrivateRoomRadialMenu.ClickButton = collision.GetComponent<Button>();
            PrivateRoomRadialMenu.ClickButton.Select();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            PrivateRoomRadialMenu.ClickButton = null;
        }
    }
}

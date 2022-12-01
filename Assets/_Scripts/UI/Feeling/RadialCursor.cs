using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialCursor : MonoBehaviour
{

    // 커서의 Collider를 이용하여 버튼을 가져와, 해당 버튼의 이미지를 RadialMenu로 전달함.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            // 이전에 선택된 버튼이 남아있다면 초기화
            if (RadialMenu._buttonOne != null)
            {
                RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(false);
                RadialMenu._buttonOne = null;
                RadialMenu._buttonOneImage = null;
            }

            // Collider와 충돌이 감지되어있는 버튼의 정보를 가져옴.
            RadialMenu._buttonOne = collision.GetComponent<Button>();
            RadialMenu._buttonOneImage = RadialMenu._buttonOne.transform.GetChild(0).GetComponent<Image>();
            RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            // TriggerExit 될때 Button의 정보를 초기화함.
            if (RadialMenu._buttonOne != null)
            {
                RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(false);
                RadialMenu._buttonOne = null;
                RadialMenu._buttonOneImage = null;
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialCursor : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            if (RadialMenu._buttonOne != null)
            {
                RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(false);
                RadialMenu._buttonOne = null;
                RadialMenu._buttonOneImage = null;
            }

            RadialMenu._buttonOne = collision.GetComponent<Button>();
            RadialMenu._buttonOneImage = RadialMenu._buttonOne.transform.GetChild(0).GetComponent<Image>();
            RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            if (RadialMenu._buttonOne != null)
            {
                RadialMenu._buttonOne.transform.GetChild(1).gameObject.SetActive(false);
                RadialMenu._buttonOne = null;
                RadialMenu._buttonOneImage = null;
            }
        }
    }


}

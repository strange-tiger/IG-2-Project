using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialCursor : MonoBehaviour
{

    private Color32 _selectedColor = new Color32(32, 32, 32, 128);
    private Color32 _unSelectedColor = new Color32(32, 32, 32, 20);


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Button"))
        {
            RadialMenu._buttonOne = collision.GetComponent<Button>();
            RadialMenu._buttonOne.image.color = _selectedColor; 
            RadialMenu._buttonOneImage = RadialMenu._buttonOne.transform.GetChild(0).GetComponent<Image>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Button"))
        {
            RadialMenu._buttonOne = null;
            RadialMenu._buttonOne.image.color = _unSelectedColor;
            RadialMenu._buttonOneImage = null;
        }
    }


}

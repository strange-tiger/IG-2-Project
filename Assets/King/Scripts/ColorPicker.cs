using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{


    [SerializeField]
    private Image _colorPalette;
    [SerializeField]
    private Image _picker;
    [SerializeField]
    private Material _skinColor;
    private Vector2 _paletteSize;
    private float[] _paletteCornerPositions = { 656f, 866f, 255f, 405f };
    
    private void Start()
    {
        _paletteSize = new Vector2(_colorPalette.GetComponent<RectTransform>().rect.width, _colorPalette.GetComponent<RectTransform>().rect.height);
       
    }
   
    private void SelectColor()
    {
       
        Debug.Log(GetColor());
        _picker.transform.localPosition = new Vector3(Mathf.Clamp(Input.mousePosition.x, _paletteCornerPositions[0], _paletteCornerPositions[1]) - 762.5f, Mathf.Clamp(Input.mousePosition.y, _paletteCornerPositions[2], _paletteCornerPositions[3]) - 330f);
        //_skinColor.color = new Color(GetColor().r, GetColor().g, GetColor().b);
        _skinColor.color = GetColor();
    }
  
    public void MousePointerDown()
    {
        SelectColor();
    }

    public void MousePointerDrag()
    {
       SelectColor();
    }


    private Color GetColor()
    {
        Vector2 colorPalettePosition = _colorPalette.transform.position;
        Vector2 pickerPosition = _picker.transform.localPosition;

        Vector2 colorPickerPosition = pickerPosition - colorPalettePosition + _paletteSize * 0.5f;
        Vector2 normalized = new Vector2(
            (colorPickerPosition.x / (_colorPalette.GetComponent<RectTransform>().rect.width)),
            (colorPickerPosition.y / (_colorPalette.GetComponent<RectTransform>().rect.height)));

        Texture2D texture = _colorPalette.mainTexture as Texture2D;
        Color circularSelectedColor = texture.GetPixelBilinear(normalized.x, normalized.y);

        return circularSelectedColor;
    }
}

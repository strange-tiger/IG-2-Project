using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{

    public Color SelectedColor;

    [SerializeField]
    private Image _colorPalette;
    [SerializeField]
    private Image _picker;
    private Vector2 _paletteSize;
    private Vector3 _pickerMovePosition;

    private void Start()
    {
        _paletteSize = new Vector2(_colorPalette.GetComponent<RectTransform>().rect.width, _colorPalette.GetComponent<RectTransform>().rect.height);
        Debug.Log(_picker.rectTransform.localPosition);
       
    }

    private void SelectColor()
    {
        Debug.Log(Input.mousePosition);
        _picker.transform.localPosition = new Vector3(Mathf.Clamp(Input.mousePosition.x, 656f, 866f), Mathf.Clamp(Input.mousePosition.y, 255f, 405f));
        
        SelectedColor = GetColor();
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
        Vector2 pickerPosition = _picker.transform.position;

        Vector2 colorPickerPosition = pickerPosition - colorPalettePosition + _paletteSize * 0.5f;
        Vector2 normalized = new Vector2(
            (colorPickerPosition.x / (_colorPalette.GetComponent<RectTransform>().rect.width)),
            (colorPickerPosition.y / (_colorPalette.GetComponent<RectTransform>().rect.height)));

        Texture2D texture = _colorPalette.mainTexture as Texture2D;
        Color circularSelectedColor = texture.GetPixelBilinear(normalized.x, normalized.y);

        return circularSelectedColor;
    }
}

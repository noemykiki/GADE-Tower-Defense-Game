using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextColorChange : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Text buttonText;  // Assign the button's text component in the Inspector
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;
    public Color normalColor = Color.white;

    // When the pointer hovers over the button
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    // When the pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }

    // When the button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonText.color = pressedColor;
    }

    // When the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonText.color = hoverColor;  // Optionally keep it on hover color after release
    }

    // Optional: Reset the color when the button is disabled
    void OnDisable()
    {
        buttonText.color = normalColor;
    }
}

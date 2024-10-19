using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Texture2D hoverCursor;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    public void OnPointerClick(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameObject.GetComponent<Image>().sprite = clickSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        gameObject.GetComponent<Image>().sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        gameObject.GetComponent<Image>().sprite = defaultSprite;
    }
}

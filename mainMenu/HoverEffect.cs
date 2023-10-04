using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color originalColor;
    public Color hoverColor;

    public Texture2D cursor;

    private TextMeshProUGUI text;

    private void Start()
    {
        text= GetComponent<TextMeshProUGUI>();
        text.color = originalColor;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = originalColor;
    }
}

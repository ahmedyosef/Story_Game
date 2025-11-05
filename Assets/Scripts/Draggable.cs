using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public Drobber gameManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        gameManager = FindObjectOfType<Drobber>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Optional: Bring to front, play sound, etc.
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ✅ CORRECT: Convert screen position to world/canvas space
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 worldPosition))
        {
            rectTransform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Pass the dropped object and drop position to GameManager
        gameManager?.OnDrop(gameObject, eventData.position);
    }
}
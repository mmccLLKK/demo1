using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 可拖动的窗口类型
/// </summary>
public class DragWindowView : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler
{
    public RectTransform dragChild;

    private void Awake()
    {
        if (!dragChild)
        {
            var child = this.transform.GetChild(0);
            dragChild = child.gameObject.GetComponent<RectTransform>();
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (!dragChild)
        {
            return;
        }

        var eventDataDelta = eventData.delta;
        dragChild.anchoredPosition += eventDataDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
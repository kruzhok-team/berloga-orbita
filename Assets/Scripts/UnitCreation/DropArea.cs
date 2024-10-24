using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropArea : MonoBehaviour, IDropHandler
{
    private void Start()
    {
        gameObject.transform.SetAsFirstSibling();
    }

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem draggableImage = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggableImage == null) return;
        draggableImage.SetDroppedInZone();
        Debug.LogError("Not implemented dragging in drop area");
        eventData.pointerDrag.GetComponent<DraggableItem>().DestroyCopyObject();

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler
{
    private bool isFilled;
    private Transform child;

    public Inventory inventory { private get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        Assign(eventData.pointerDrag.transform);
    }

    public void Assign(Transform obj)
    {
        if (isFilled)
            return;
        child = obj;
        child.transform.parent = transform;
        child.GetComponent<DraggableItem>().SetSlot(this);
        child.GetComponent<RectTransform>().anchoredPosition = Vector2.one * 0.5f;
        isFilled = true;
        inventory.AddItemToSlot(this);
    }

    public void Remove()
    {
        if (!isFilled)
            return;
        isFilled = false;
        if (child)
            child.transform.parent = transform.root;
        inventory.RemoveItemFromSlot(this);
    }
}

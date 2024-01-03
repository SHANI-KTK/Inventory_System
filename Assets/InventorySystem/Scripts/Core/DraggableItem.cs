using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Text stackText;
    [SerializeField] private Text nameText;
    [SerializeField] private Image rarityBG;

    private Item item;
    private Slot targetSlot;
    private Inventory inventory;

    public Item getitem => item;
    public void SetItem(Item item, Inventory inventory)
    {
        this.inventory = inventory;
        this.item = item;
        stackText.text = item._saveData._stack.ToString();
        nameText.text = item._saveData._nameID;
        iconImage.sprite = this.item.getIcon;
        rarityBG.color = inventory.GetRarityColor(this.item._saveData._rarity);
    }
    public void SetSlot(Slot targetSlot)
    {
        this.targetSlot = targetSlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rarityBG.raycastTarget = false;
        targetSlot.Remove();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        targetSlot.Assign(transform);
        rarityBG.raycastTarget = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerDrag;
        KeyValuePair<int, bool> stack = inventory.StackItem(item, obj.GetComponent<DraggableItem>().item);
        stackText.text = stack.Key.ToString();
        if (stack.Value)
            Destroy(obj);
    }
}

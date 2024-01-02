using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIHandler : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GridLayoutGroup itemGrid; // Reference to the GridLayoutGroup component
    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemQuantityText;
    public GameObject inventorySlotPrefab; // Prefab for the inventory slot

    private List<GameObject> inventorySlots = new List<GameObject>();

    private void Start()
    {
        // Optionally, initialize UI elements or perform setup
        PopulateInventoryUI();
    }

    private void PopulateInventoryUI()
    {
        // Clear existing inventory slots
        ClearInventoryUI();

        // Get the list of items from the InventoryManager
        List<BaseItem> items = new List<BaseItem>(InventoryManager.Instance.GetInventory().Keys);

        // Create UI slots for each item
        foreach (BaseItem item in items)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, itemGrid.transform);
            Button slotButton = slot.GetComponent<Button>();
            slotButton.onClick.AddListener(() => OnItemClick(item));

            Image slotImage = FindImageInChildren(slot.transform, "ItemSprite");
            if (slotImage != null && item.itemSprite != null)
            {
                slotImage.sprite = item.itemSprite;
            }

            inventorySlots.Add(slot);
        }
    }

    private void ClearInventoryUI()
    {
        // Destroy existing inventory slots
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }

        inventorySlots.Clear();
    }

    public void UpdateItemUI(BaseItem item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        itemQuantityText.text = $"Quantity: {InventoryManager.Instance.GetQuantity(item)}";
    }

    public void OpenInventory()
    {
        inventoryPanel.SetActive(true);
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    public void OnItemClick(BaseItem item)
    {
        UpdateItemUI(item);
    }

    private Image FindImageInChildren(Transform parent, string name)
    {
        Transform child = parent.Find(name);

        if (child != null)
        {
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                return image;
            }
        }

        // Recursively search in children
        foreach (Transform childTransform in parent)
        {
            Image imageInChildren = FindImageInChildren(childTransform, name);
            if (imageInChildren != null)
            {
                return imageInChildren;
            }
        }

        return null; // Image not found
    }
}

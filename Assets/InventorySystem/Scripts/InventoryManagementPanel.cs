using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagementPanel : MonoBehaviour
{
    public Dropdown itemDropdown;
    public InputField quantityInput;
    public Button addButton;
    public Button removeButton;

    public Text itemNameText;
    public Text itemDescriptionText;
    public Text itemQuantityText;

    private void Start()
    {
        PopulateDropdown();

        addButton.onClick.AddListener(AddItemToInventory);
        removeButton.onClick.AddListener(RemoveItemFromInventory);
    }

    private void PopulateDropdown()
    {
        // Retrieve the list of items from the InventoryManager
        List<BaseItem> items = new List<BaseItem>(InventoryManager.Instance.GetInventory().Keys);

        // Populate the dropdown with item names
        itemDropdown.ClearOptions();
        List<string> itemNames = new List<string>();
        foreach (BaseItem item in items)
        {
            itemNames.Add(item.itemName);
        }
        itemDropdown.AddOptions(itemNames);
    }

    private void AddItemToInventory()
    {
        // Get the selected item from the dropdown
        string selectedItemName = itemDropdown.options[itemDropdown.value].text;
        BaseItem selectedBaseItem = GetItemByName(selectedItemName);

        if (selectedBaseItem != null)
        {
            // Get the quantity from the input field (default to 1 if empty)
            int quantity = string.IsNullOrEmpty(quantityInput.text) ? 1 : int.Parse(quantityInput.text);

            // Add the item to the inventory
            InventoryManager.Instance.AddItem(selectedBaseItem, quantity);

            // Update UI
            UpdateItemUI(selectedBaseItem);
        }
    }

    private void RemoveItemFromInventory()
    {
        // Get the selected item from the dropdown
        string selectedItemName = itemDropdown.options[itemDropdown.value].text;
        BaseItem selectedBaseItem = GetItemByName(selectedItemName);

        if (selectedBaseItem != null)
        {
            // Get the quantity from the input field (default to 1 if empty)
            int quantity = string.IsNullOrEmpty(quantityInput.text) ? 1 : int.Parse(quantityInput.text);

            // Remove the item from the inventory
            InventoryManager.Instance.RemoveItem(selectedBaseItem, quantity);

            // Update UI
            UpdateItemUI(selectedBaseItem);
        }
    }

    private void UpdateItemUI(BaseItem item)
    {
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
        itemQuantityText.text = $"Quantity: {InventoryManager.Instance.GetQuantity(item)}";
    }

    private BaseItem GetItemByName(string itemName)
    {
        // Retrieve the list of items from the InventoryManager
        List<BaseItem> items = new List<BaseItem>(InventoryManager.Instance.GetInventory().Keys);

        // Find the item with the specified name
        foreach (BaseItem item in items)
        {
            if (item.itemName == itemName)
            {
                return item;
            }
        }

        return null; // Item not found
    }
}

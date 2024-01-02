using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager instance;
    public static InventoryManager Instance => instance;

    private Dictionary<BaseItem, int> inventory = new Dictionary<BaseItem, int>();
    private Dictionary<EquipmentSlot, EquipmentItem> equippedItems = new Dictionary<EquipmentSlot, EquipmentItem>();
    private string saveFilePath;

    private void Awake()
    {
        instance = this;
        saveFilePath = Application.persistentDataPath + "/inventory.json";

        // Check if save file exists before loading
        if (File.Exists(saveFilePath))
        {
            LoadInventory();
        }
        else
        {
            // Load default inventory if save file doesn't exist
            LoadDefaultInventory();
        }
    }

    public void AddItem(BaseItem item, int quantity = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory.Add(item, quantity);
        }

        if (item is IItemInteractable interactableItem)
        {
            interactableItem.Interact(this);
        }
    }

    public void RemoveItem(BaseItem item, int quantity = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= quantity;

            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
        }
    }

    public Dictionary<BaseItem, int> GetInventory()
    {
        return inventory;
    }

    public int GetQuantity(BaseItem item)
    {
        return inventory.ContainsKey(item) ? inventory[item] : 0;
    }

    public bool HasItem(BaseItem item)
    {
        return inventory.ContainsKey(item) && inventory[item] > 0;
    }

    public void SaveInventory()
    {
        JsonUtilityHelper.SaveToJson(saveFilePath, inventory);
    }

    public void LoadInventory()
    {
        inventory = JsonUtilityHelper.LoadFromJson<Dictionary<BaseItem, int>>(saveFilePath);
    }

    public bool IsItemEquipped(EquipmentItem item)
    {
        return equippedItems.ContainsKey(item.equipmentSlot) && equippedItems[item.equipmentSlot] == item;
    }

    public bool CanEquipInSlot(EquipmentSlot slot)
    {
        return !equippedItems.ContainsKey(slot);
    }

    public void EquipItem(EquipmentItem item)
    {
        equippedItems[item.equipmentSlot] = item;
        // Optionally, you can apply equipment modifiers or other effects here
    }

    public void UnequipItem(EquipmentItem item)
    {
        equippedItems.Remove(item.equipmentSlot);
    }
    private void LoadDefaultInventory()
    {
        // Use reflection to find all types derived from BaseItem
        IEnumerable<Type> baseItemTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(BaseItem)) && !type.IsAbstract);

        // Instantiate and add default items for each type
        foreach (Type baseItemType in baseItemTypes)
        {
            BaseItem defaultItem = (BaseItem)Activator.CreateInstance(baseItemType);

            // Add default item to the inventory
            AddItem(defaultItem, 1);
        }
    }
}
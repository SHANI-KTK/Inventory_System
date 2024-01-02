using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Equipment")]
public class EquipmentItem : BaseItem, IItemInteractable
{
    public EquipmentSlot equipmentSlot;
    public ModifierType modifierType;
    public float modifierValue;

    public void Interact(InventoryManager inventoryManager)
    {
        if (CanEquip(inventoryManager))
        {
            EquipItem(inventoryManager);
        }
        else if (IsEquipped(inventoryManager))
        {
            UnequipItem(inventoryManager);
        }
        else
        {
            Debug.LogWarning($"Cannot equip {itemName}. Missing dependencies or restricted items.");
        }
    }

    private bool CanEquip(InventoryManager inventoryManager)
    {
        foreach (var dependency in itemDependencies)
        {
            if (!inventoryManager.HasItem(dependency))
            {
                return false;
            }
        }

        foreach (var restriction in itemRestrictions)
        {
            if (inventoryManager.HasItem(restriction))
            {
                return false;
            }
        }

        // Implement additional logic for equipment-specific restrictions if needed

        return true;
    }

    private bool IsEquipped(InventoryManager inventoryManager)
    {
        return inventoryManager.IsItemEquipped(this);
    }

    private void EquipItem(InventoryManager inventoryManager)
    {
        if (inventoryManager.CanEquipInSlot(equipmentSlot))
        {
            inventoryManager.EquipItem(this);
            Debug.Log($"Equipping {itemName} in {equipmentSlot} slot.");
        }
        else
        {
            Debug.LogWarning($"Cannot equip {itemName}. Slot already occupied.");
        }
    }

    private void UnequipItem(InventoryManager inventoryManager)
    {
        inventoryManager.UnequipItem(this);
        Debug.Log($"Unequipping {itemName} from {equipmentSlot} slot.");
    }
}

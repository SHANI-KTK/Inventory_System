using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Consumable")]
public class ConsumableItem : BaseItem, IItemInteractable
{
    public ConsumableEffect consumableEffect;

    public void Interact(InventoryManager inventoryManager)
    {
        if (CanUseItem(inventoryManager))
        {
            ApplyConsumableEffect(inventoryManager);
            inventoryManager.RemoveItem(this);
        }
        else
        {
            Debug.LogWarning($"Cannot use {itemName}. Missing dependencies or restricted items.");
        }
    }

    private bool CanUseItem(InventoryManager inventoryManager)
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

        return true;
    }

    private void ApplyConsumableEffect(InventoryManager inventoryManager)
    {
        switch (consumableEffect)
        {
            case ConsumableEffect.Health:
                // Implement logic to increase player's health
                Debug.Log($"Using {itemName} to restore health.");
                break;

            case ConsumableEffect.Stamina:
                // Implement logic to restore player's stamina
                Debug.Log($"Using {itemName} to restore stamina.");
                break;

            // Add more cases for additional consumable effects...

            default:
                break;
        }
        // Optionally, you can also apply global effects or bonuses here
    }
}

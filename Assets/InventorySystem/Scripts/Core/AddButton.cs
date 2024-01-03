using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;

    private void Start()
    {
        GetComponent<Image>().sprite = item.getIcon;
    }

    public void ADD()
    {
        if (item.GetType() == typeof(Consumeable))
        {
            Consumeable consumeable = new Consumeable(item);
            consumeable._saveData._rarity = (Rarity)Random.Range(0, 4);
            inventory.FindEmptySpaceToAddItem(consumeable);
        }
        else if (item.GetType() == typeof(Equipment))
        {
            Equipment equipment = new Equipment(item);
            equipment._saveData._rarity = (Rarity)Random.Range(0, 4);
            inventory.FindEmptySpaceToAddItem(equipment);
        }
    }
}

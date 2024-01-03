using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Inventory", menuName = "Game/Inventory System/Inventory", order = 0)]
public class Inventory : MonoBehaviour
{
    [SerializeField] Data data;
    [SerializeField] Item[] Items;
    [SerializeField] Color[] rarityColors;
    [SerializeField] private GameObject draggableItemPrefab;
    [SerializeField] private int bp_size;

    private class Pack
    {
        public DraggableItem draggableItem;
        public Slot slot;
    }
    private Pack[] equipedItems;
    private Pack[] bagPack;

    private DraggableItem draggeditem;

    public int bagPackSize => bp_size;
    public Color GetRarityColor(Rarity rarity) => rarityColors[(int)rarity];

    public void Init(GridLayoutGroup bg_Grid, GridLayoutGroup ei_Grid)
    {
        InitArray(ref bagPack, bp_size);
        InitArray(ref equipedItems, 4);
        SpawnCells(Vector2.one * 120, bp_size, bg_Grid, ref bagPack);
        SpawnCells(Vector2.one * 120, 4, ei_Grid, ref equipedItems);
        LoadItems(equipedItems, "Equipped Items");
        LoadItems(bagPack, "Bag Pack");
    }

    private void InitArray<T>(ref T[] array, int size)
    {
        array = new T[size];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Activator.CreateInstance<T>();
        }
    }

    #region Load and Save Functions
    private void LoadItems(Pack[] pack, string fileName)
    {
        SaveData[] itemsToLoad = GetSaveData(pack);
        data.LoadData(ref itemsToLoad, fileName);
        for (int i = 0; i < pack.Length; i++)
        {
            if (!string.IsNullOrEmpty(itemsToLoad[i]._nameID))
            {
                Item item = (Item)Activator.CreateInstance(itemsToLoad[i]._type, new object[] { GetItemPreset(itemsToLoad[i]._type, itemsToLoad[i]._nameID) });
                item._saveData = new SaveData(itemsToLoad[i]);
                AddItem(item, i, pack);
            }
        }
    }
    private SaveData[] GetSaveData(Pack[] pack)
    {
        SaveData[] saveData = new SaveData[pack.Length];
        for (int i = 0; i < pack.Length; i++)
        {
            if (pack[i].draggableItem != null)
                saveData[i] = pack[i].draggableItem.getitem._saveData;
            else
                saveData[i] = new SaveData();
        }
        return saveData;
    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus == false)
        {
            data.SaveData(GetSaveData(equipedItems), "Equipped Items");
            data.SaveData(GetSaveData(bagPack), "Bag Pack");
        }
    }

    private Item GetItemPreset(Type type, string nameID)
    {
        foreach (Item item in Items)
        {
            if (item.GetType() == type && nameID == item._saveData._nameID)
                return item;
        }
        return null;
    }
    #endregion

    #region spawning of cells of inventory
    private void SpawnCells(Vector2 cellSize, float noOfCells, GridLayoutGroup grid, ref Pack[] container)
    {
        grid.cellSize = cellSize;
        Type[] components = new Type[] { typeof(RectTransform), typeof(Image), typeof(Slot) };
        for (int i = 0; i < noOfCells; i++)
        {
            GameObject obj = new GameObject("slot " + (i + 1), components);
            obj.transform.SetParent(grid.transform);
            container[i].slot = obj.GetComponent<Slot>();
            container[i].slot.inventory = this;
        }
    }
    #endregion

    #region Inventory Traversing Operations
    private void AddItem(Item item, int index, Pack[] pack)
    {
        GameObject obj = Instantiate(draggableItemPrefab);
        DraggableItem d_Item = obj.GetComponent<DraggableItem>();
        d_Item.SetItem(item, this);
        draggeditem = d_Item;
        pack[index].slot.Assign(obj.transform);
    }

    public void FindEmptySpaceToAddItem(Item item)
    {
        for (int i = 0; i < bagPack.Length; i++)
        {
            if (bagPack[i].draggableItem == null)
            {
                AddItem(item, i, bagPack);
                break;
            }
        }
    }

    public KeyValuePair<int, bool> StackItem(Item stackableItem, Item dropedItem)
    {
        bool canStack = stackableItem.IsEqual(dropedItem) && stackableItem._saveData._isStackable && dropedItem._saveData._isStackable;
        if (canStack)
        {
            stackableItem._saveData._stack += dropedItem._saveData._stack;
        }
        return new KeyValuePair<int, bool>(stackableItem._saveData._stack, canStack);
    }

    public void RemoveItemFromSlot(Slot slot)
    {
        Pack pack = (Pack)GetBySlot(slot);
        draggeditem = pack.draggableItem;
        pack.draggableItem = null;
    }

    public void AddItemToSlot(Slot slot)
    {
        Pack pack = GetBySlot(slot);
        pack.draggableItem = draggeditem;
    }

    private Pack GetBySlot(Slot slot)
    {
        for (int i = 0; i < bagPack.Length; i++)
        {
            if (bagPack[i].slot == slot)
                return bagPack[i];
        }
        for (int i = 0; i < equipedItems.Length; i++)
        {
            if (equipedItems[i].slot == slot)
                return equipedItems[i];
        }
        return null;
    }
    public void Sort(int type)
    {
        List<DraggableItem> d_items = new List<DraggableItem>();
        for (int i = 0; i < bagPack.Length; i++)
        {
            if (bagPack[i].draggableItem != null)
            {
                d_items.Add(bagPack[i].draggableItem);
                bagPack[i].slot.Remove();
            }
        }
        if (type == 0)
            d_items.Sort((x, y) => x.getitem._saveData._nameID.CompareTo(y.getitem._saveData._nameID));
        else if (type == 1)
            d_items.Sort((x, y) => x.getitem._saveData._stack.CompareTo(y.getitem._saveData._stack));
        ReArrangeItems(d_items);
    }
    private void ReArrangeItems(List<DraggableItem> d_items)
    {
        for (int i = 0; i < d_items.Count; i++)
        {
            draggeditem = d_items[i];
            bagPack[i].slot.Assign(draggeditem.transform);
        }
    }
    public void ShowType(int type)
    {
        List<Type> types = new List<Type>();
        if (type == 1 || type == 0)
            types.Add(typeof(Consumeable));
        if (type == 2 || type == 0)
            types.Add(typeof(Equipment));
        ShowSpecificType(types);
    }
    private void ShowSpecificType(List<Type> type)
    {
        for (int i = 0; i < bagPack.Length; i++)
        {
            if (bagPack[i].draggableItem != null)
            {
                if (type.Contains(bagPack[i].draggableItem.getitem._saveData._type))
                    bagPack[i].draggableItem.GetComponent<CanvasGroup>().alpha = 1;
                else
                    bagPack[i].draggableItem.GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }
    #endregion
}

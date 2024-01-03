using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    //[SerializeField] GameObject draggablePrefab;
    [SerializeField] Inventory inventory;

    [Header("Bag Pack")]
    //[SerializeField] private Vector2 bp_cellSize;
    [SerializeField] private GridLayoutGroup bp_Grid;

    [Header("Equiped Items")]
    //[SerializeField] private Vector2 ei_cellSize;
    //[SerializeField] private float ei_noOfCells;
    [SerializeField] private GridLayoutGroup ei_Grid;

    //private List<Slot> bagPack = new List<Slot>();
    //private List<Slot> equippedItems = new List<Slot>();

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inventory.Init(bp_Grid, ei_Grid);
        //inventory.Additem(new Consumeable("Apple", 0));
        // SpawnCells(bp_cellSize, inventory.bagPackSize, bp_Grid, ref bagPack);
        //SpawnCells(ei_cellSize, ei_noOfCells, ei_Grid, ref equippedItems);
        //UpdateUI();
    }

    //public void AddItem(Item item)
    //{
    //    Nullable<int> index = inventory.Additem(item);
    //    if (index != null)
    //    {
    //        bagPack[(int)index].Assign(draggablePrefab);
    //    }
    //}

    //public void Moveitem(Item item)
    //{

    //}
    //public void UpdateUI()
    //{
    //    for (int i = 0; i < inventory.bagPackSize; i++)
    //    {
    //        if (inventory.Getitem(i) != null)
    //        {
    //            bagPack[i].Assign(draggablePrefab);
    //        }
    //    }
    //}

    //private void SpawnCells(Vector2 cellSize, float noOfCells, GridLayoutGroup grid, ref List<Slot> container)
    //{
    //    grid.cellSize = cellSize;
    //    Type[] components = new Type[] { typeof(RectTransform), typeof(Image), typeof(Slot) };
    //    for (int i = 0; i < noOfCells; i++)
    //    {
    //        GameObject obj = new GameObject("slot " + (i + 1), components);
    //        obj.transform.SetParent(grid.transform);
    //        container.Add(obj.GetComponent<Slot>());
    //    }
    //}
}

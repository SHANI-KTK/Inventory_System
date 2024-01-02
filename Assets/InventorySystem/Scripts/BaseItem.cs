using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class BaseItem : ScriptableObject
{
    public string itemName;
    public string description;
    public float weight;
    public int value;
    public Sprite itemSprite;

    public List<BaseItem> itemDependencies = new List<BaseItem>();
    public List<BaseItem> itemRestrictions = new List<BaseItem>();
}

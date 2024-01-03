using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity { COMMON, UNCOMMON, RARE, EPIC }
public abstract class Item : ScriptableObject
{
    [SerializeField] private SaveData saveData;
    [SerializeField] private Sprite icon;

    public SaveData _saveData { set => saveData = value; get => saveData; }

    public Sprite getIcon => icon;

    public bool IsEqual(Item item) => saveData._nameID == item.saveData._nameID && GetType() == item.GetType() && saveData._rarity == item.saveData._rarity;

    public Item(Item item)
    {
        saveData = new SaveData(item.saveData);
        saveData._type = item.GetType();
        icon = item.icon;
        saveData._stack = 1;
    }

    public abstract void Use();
}
[System.Serializable]
public partial class SaveData
{
    [SerializeField] private string nameID;
    [SerializeField] private Rarity rarity;
    [SerializeField] private int stack;
    [SerializeField] private bool isStackable;
    [SerializeField] private string type;

    public SaveData() { }
    public SaveData(SaveData saveData)
    {
        nameID = saveData.nameID;
        rarity = saveData.rarity;
        stack = saveData.stack;
        isStackable = saveData.isStackable;
        type = saveData.type;
    }
    public System.Type _type { get => System.Type.GetType(type); set => type = value.ToString(); }
    public bool _isStackable { set => isStackable = value; get => isStackable; }
    public int _stack { set => stack = value; get => stack; }
    public string _nameID { get => nameID; set => nameID = value; }
    public Rarity _rarity { set => rarity = value; get => rarity; }

}
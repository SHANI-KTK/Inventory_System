using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Game/Inventory System/Types/Equipment", order = 1)]
public class Equipment : Item
{
    public Equipment(Item item) : base(item) { }

    public override void Use()
    {
    }
}

public partial class SaveData
{
}
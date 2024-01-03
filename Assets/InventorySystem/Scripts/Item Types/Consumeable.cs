using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Game/Inventory System/Types/Consumable", order = 0)]
public class Consumeable : Item
{
    public Consumeable(Item item) : base(item) { }

    public override void Use()
    {
    }
}

public partial class SaveData
{
}

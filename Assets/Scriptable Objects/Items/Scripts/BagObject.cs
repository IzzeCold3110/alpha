using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag Object", menuName = "Inventory System/Items/Bag")]
public class BagObject : ItemObject
{
    void Awake()
    {
        type = ItemType.Equipment;
        subtype = SubItemType.Bag;
    }
}

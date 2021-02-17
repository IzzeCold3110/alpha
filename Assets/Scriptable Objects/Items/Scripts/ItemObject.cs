using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Equipment,
    Default
}

public enum SubItemType
{
    None,
    Bag,
    Clothing
}

public abstract class ItemObject : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public SubItemType subtype;
    public int bagSlots = -1;
    [TextArea(15, 20)]
    public string description;

}

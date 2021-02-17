using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    public List<ItemObject> items;
    public int BagSlots = 0;

    public void LoadItems()
    {
        string[] files = Directory.GetFiles("Assets/Resources/Items", "*.asset", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            if (File.Exists(file))
            {
                ItemObject item = LoadItemFromFilepath(file);
                if (!items.Contains(item))
                {
                    items.Add(item);
                }
            }

        }
        Debug.Log("loaded "+ items.Count + " items");
    }

    public ItemObject LoadItemFromFilepath(string filepath)
    {
        string file_ = filepath.Replace("Assets/Resources/", "");
        return Resources.Load(file_.Replace(".asset", "")) as ItemObject;
    }

    public delegate void ChangeBagSlotsEvent(int bagSlots); //I do declare!
    public static event ChangeBagSlotsEvent changeBagSlotsEvent;  // create an event variable 

    public delegate void AddItemEvent(Item item, int amount); //I do declare!
    public static event AddItemEvent changeItemEvent;  // create an event variable 

    private void Start()
    {
        inventory = (InventoryObject)ScriptableObject.CreateInstance("InventoryObject");
        items = new List<ItemObject>();

        LoadItems();
        
    }

    private void OnEnable()
    {
        changeBagSlotsEvent += BagSlotsChanged; // subscribing to the event. 
        changeItemEvent += PlayerItemsChanged; // subscribing to the event. 
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        if(item)
        {
            Debug.Log(item.item.type);
            Debug.Log(item.item.subtype);
            if (BagSlots == 0)
            {


                if (item.item.type == ItemType.Equipment && item.item.subtype == SubItemType.Bag)
                {
                    BagSlots += item.item.bagSlots;
                    if (changeBagSlotsEvent != null) // checking if anyone is on the other line.
                        changeBagSlotsEvent(item.item.bagSlots);

                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.LogError("Need a bag");
                }
            } else if (BagSlots > 0)
            {
                if (item.item.type == ItemType.Equipment && item.item.subtype == SubItemType.Bag)
                {
                    BagSlots += item.item.bagSlots;
                    if (changeBagSlotsEvent != null) // checking if anyone is on the other line.
                        changeBagSlotsEvent(item.item.bagSlots);

                    Destroy(other.gameObject);
                }
                else
                {
                    changeItemEvent(item, 1);
                    inventory.AddItem(item.item, 1);
                    Destroy(other.gameObject);
                }
                
            }
        
        }
    }
    void BagSlotsChanged(int newBagSlots)
    {
        Debug.Log("BagSlotsChanged now = " + newBagSlots);  // This will trigger anytime you call MoreApples() on ClassA
    }

    void PlayerItemsChanged(Item item, int amount)
    {
        Debug.Log("PlayerItemsChanged "+item.item.name+" now = " + amount);  // This will trigger anytime you call MoreApples() on ClassA
    }

}

using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Items> items = new List<Items>(); // Change Item to Items
    public int money;

    public void RemoveItem(Items item)
    {
        items.Remove(item);
    }

    public void AddItem(Items item)
    {
        items.Add(item);
    }
}

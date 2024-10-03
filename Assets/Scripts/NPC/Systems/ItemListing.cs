using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemListing : MonoBehaviour
{
	public ShopSystem shopSystem;
	public Items item;
	public Image icon;
	public TMP_Text itemName;
	public TMP_Text price;
	ListingMode mode;

	//	Mode 0 = sell, mode 1 = buy
	public void ListItem(Items item, ListingMode mode)
	{
		this.mode = mode;
		this.item = item;

        if (item == null)
        {
            Debug.LogError("Item is null!");
            return;
        }

        icon.sprite = item.icon;
		price.text = item.price.ToString();
		itemName.text = item.name;
	}

	public void ButtonClicked()
	{
		if (mode.Equals(ListingMode.Sell))
		{
			shopSystem.SellToShop(item);
		}
		else if (mode.Equals(ListingMode.Buy))
		{
			shopSystem.BuyFromShop(item);
		}
	}

	public enum ListingMode
	{
		Buy,
		Sell
	}
}

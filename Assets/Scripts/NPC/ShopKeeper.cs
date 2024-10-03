using System;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
	public ShopSystem shopSystem;

	bool inRange = false;
	public string playerTag;
	public GameObject tooltip;

	public string shopName;
	public bool canSellTo = true;
	public bool finiteMoney = true;
	public bool finiteItems = true;
	public Inventory shopInventory;

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag.Equals(playerTag))
		{
			ChangeState(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.transform.tag.Equals(playerTag))
		{
			ChangeState(false);
			try
			{
				if (shopSystem.shopkeeper.GetInstanceID() == this.GetInstanceID())
				{
					shopSystem.CloseShop();
				}
			}
			catch (Exception e) { }
		}
	}

	void ChangeState(bool state)
	{
		inRange = state;
		tooltip.SetActive(state);
	}

	public bool StartTrade()
	{
		if (!inRange)
		{
			return false;
		}
		shopSystem.OpenShop(this);
		return true;
	}
}

using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public bool shopOpen = false;
    public Shopkeeper shopkeeper;
    public Inventory playerInventory;

    [Header("UI")]
    public GameObject shopUI;
    public TMP_Text playerMoney;
    public TMP_Text shopName;
    public TMP_Text shopMoney;
    public RectTransform playerItems;
    public RectTransform shopItems;
    public GameObject itemListingPrefab;

    public void OpenShop(Shopkeeper keeper)
    {
        shopOpen = true;
        shopkeeper = keeper;
        shopName.text = shopkeeper.shopName;
        UpdateMoneyUI();
        ClearListings();
        LoadPlayerItems();
        LoadShopItems();
        shopUI.SetActive(true);
    }

    void ClearListings()
    {
        foreach (RectTransform listing in playerItems.transform)
        {
            Destroy(listing.gameObject);
        }

        foreach (RectTransform listing in shopItems.transform)
        {
            Destroy(listing.gameObject);
        }
    }

    void LoadPlayerItems()
    {
        foreach (Items item in playerInventory.items)
        {
            AddItemToList(playerItems, item, ItemListing.ListingMode.Sell);
        }
    }

    void LoadShopItems()
    {
        foreach (Items item in shopkeeper.shopInventory.items)
        {
            AddItemToList(shopItems, item, ItemListing.ListingMode.Buy);
        }
    }

    public void AddItemToList(RectTransform list, Items item, ItemListing.ListingMode mode)
    {
        GameObject clone = Instantiate(itemListingPrefab, itemListingPrefab.transform.position, Quaternion.identity);
        ItemListing listing = clone.GetComponent<ItemListing>();
        listing.ListItem(item, mode);
        listing.shopSystem = this;
        RectTransform rect = clone.GetComponent<RectTransform>();

        //	This shouldn't be hardcoded but it's okay for now...
        rect.sizeDelta = new Vector2(512, 80);

        clone.transform.SetParent(list, false);
    }

    public void RemoveItemFromList(RectTransform list, Items item)
    {
        foreach (RectTransform listing in list.transform)
        {
            if (listing.GetComponent<ItemListing>().item.id == item.id)
            {
                Destroy(listing.gameObject);
                break;
            }
        }
    }

    public void SellToShop(Items item)
    {
        if (!shopkeeper.canSellTo)
        {
            return;
        }
        if (shopkeeper.finiteMoney)
        {
            if (shopkeeper.shopInventory.money - item.price < 0)
            {
                return;
            }
        }
        playerInventory.money += item.price;
        playerInventory.RemoveItem(item);
        RemoveItemFromList(playerItems, item);
        if (shopkeeper.finiteItems)
        {
            shopkeeper.shopInventory.AddItem(item);
            AddItemToList(shopItems, item, ItemListing.ListingMode.Buy);
        }
        if (shopkeeper.finiteMoney)
        {
            shopkeeper.shopInventory.money -= item.price;
        }
        UpdateMoneyUI();
    }

    public void BuyFromShop(Items item)
    {
        if (playerInventory.money - item.price < 0)
        {
            return;
        }

        playerInventory.money -= item.price;
        if (shopkeeper.finiteItems)
        {
            shopkeeper.shopInventory.RemoveItem(item);
            RemoveItemFromList(shopItems, item);
        }
        if (shopkeeper.finiteMoney)
        {
            shopkeeper.shopInventory.money += item.price;
        }
        playerInventory.AddItem(item);
        AddItemToList(playerItems, item, ItemListing.ListingMode.Sell);
        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        playerMoney.text = playerInventory.money.ToString();
        if (shopkeeper.finiteMoney)
        {
            shopMoney.text = shopkeeper.shopInventory.money.ToString();
        }
        else
        {
            shopMoney.text = "oo";
        }
    }

    public void CloseShop()
    {
        shopkeeper = null;
        shopUI.SetActive(false);
        shopOpen = false;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public SpawnerScript spawner;
    public PlayerController PlayerController;
    public WandShooting WandShooting;

    //public Bullet Bullet;
    public double damageBuff;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }



    public event Action<int> OnBuyCannonTower;
    public event Action<int> OnBuyMageTower;
    public event Action<int> OnBuyTrapTower;
    public event Action<int> OnBuyHealthUp;
    public event Action<int> OnBuyManaUp;
    public event Action<int> OnBuyDamageUp;
    public event Action<int> OnBuyRegenUp;
    public event Action<int> OnBuySpeedUp;
    public event Action<int> OnBuyHealthS;
    public event Action<int> OnBuyHealthL;
    public event Action<int> OnBuyHealthM;
    public event Action<int> OnBuyLaserWand;
    public event Action<int> OnBuyIceWand;
    public event Action<int> OnBuyExplosiveWand;



    //----------------------------------------------------
    //NPC Event Buys
    //----------------------------------------------------
    public void BuyCannonTowerLT()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought1 = true;
            OnBuyCannonTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Cannon Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyCannonTowerRT()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought3 = true;
            OnBuyCannonTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Cannon Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyCannonTowerLB()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought4 = true;
            OnBuyCannonTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Cannon Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyCannonTowerRB()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought6 = true;
            OnBuyCannonTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Cannon Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyMageTowerT()
    {
        if (PlayerController.goldCount >= 100) // Check if player has enough gold
        {
            PlayerController.goldCount -= 100;
            PlayerController.Bought2 = true;
            OnBuyMageTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Mage Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyMageTowerB()
    {
        if (PlayerController.goldCount >= 100) // Check if player has enough gold
        {
            PlayerController.goldCount -= 100;
            PlayerController.Bought5 = true;
            OnBuyMageTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Mage Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void BuyTrapTowerL()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought7 = true;
            OnBuyTrapTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Wall Traps bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void BuyTrapTowerR()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50;
            PlayerController.Bought8 = true;
            OnBuyTrapTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Wall Traps bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void BuyHealthUp()
    {
        if (PlayerController.goldCount >= 30) // Check if player has enough gold
        {
            PlayerController.goldCount -= 30;
            OnBuyHealthUp?.Invoke(30); // Triggers to buy Upgrade
            Debug.Log("Health Upgrade bought! Remaining gold: " + PlayerController.goldCount);

            PlayerController.maxHealth += 20; // Increases player max health
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyManaUp()
    {
        if (PlayerController.goldCount >= 30) // Check if player has enough gold
        {
            PlayerController.goldCount -= 30;
            OnBuyManaUp?.Invoke(30); // Triggers to buy Upgrade
            Debug.Log("Mana upgrade bought! Remaining gold: " + PlayerController.goldCount);

            WandShooting.maxMana += 30; //Increase the max mana by 30 for the bullets
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

    public void BuyDamageUp()
    {
        if (PlayerController.goldCount >= 30) // Check if player has enough gold
        {
            PlayerController.goldCount -= 30;
            OnBuyDamageUp?.Invoke(30); // Triggers to buy Upgrade
            Debug.Log("Damage upgrade bought! Remaining gold: " + PlayerController.goldCount);

            damageBuff += 2; //Increase the damage multiplyer for the bullets
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyRegenUp()
    {
        if (PlayerController.goldCount >= 30) // Check if player has enough gold
        {
            PlayerController.goldCount -= 30;
            OnBuyRegenUp?.Invoke(30); // Triggers to buy Upgrade
            Debug.Log("Regen upgrade bought! Remaining gold: " + PlayerController.goldCount);

            //Need to do
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuySpeedUp()
    {
        if (PlayerController.goldCount >= 30) // Check if player has enough gold
        {
            PlayerController.goldCount -= 30;
            OnBuySpeedUp?.Invoke(30); // Triggers to buy Upgrade
            Debug.Log("Speed upgrade bought! Remaining gold: " + PlayerController.goldCount);

            PlayerController.speed += 0.5f; //Increases player speed
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyHealthS()
    {
        if (PlayerController.goldCount >= 15) // Check if player has enough gold
        {
            PlayerController.goldCount -= 15;
            OnBuyHealthS?.Invoke(15); // Triggers to buy Health
            Debug.Log("Health small bought! Remaining gold: " + PlayerController.goldCount);

            PlayerController.currHealth += 20;
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyHealthL()
    {
        if (PlayerController.goldCount >= 40) // Check if player has enough gold
        {
            PlayerController.goldCount -= 40;
            OnBuyHealthL?.Invoke(40); // Triggers to buy Health
            Debug.Log("Health Large bought! Remaining gold: " + PlayerController.goldCount);

            PlayerController.currHealth += 50;
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyHealthM()
    {
        if (PlayerController.goldCount >= 150) // Check if player has enough gold
        {
            PlayerController.goldCount -= 150;
            OnBuyHealthM?.Invoke(150); // Triggers to buy Health
            Debug.Log("Health Max bought! Remaining gold: " + PlayerController.goldCount);

            PlayerController.currHealth += PlayerController.maxHealth;
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyLaserWand()
    {
        if (PlayerController.goldCount >= 200) // Check if player has enough gold
        {
            PlayerController.goldCount -= 200;
            PlayerController.LaserBought = true;
            OnBuyLaserWand?.Invoke(200); // Triggers to buy Weapon
            Debug.Log("Laser Wand bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyIceWand()
    {
        if (PlayerController.goldCount >= 400) // Check if player has enough gold
        {
            PlayerController.goldCount -= 400;
            PlayerController.IceBought = true;
            OnBuyIceWand?.Invoke(400); // Triggers to buy Weapon
            Debug.Log("Ice Wand bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
    public void BuyExplosiveWand()
    {
        if (PlayerController.goldCount >= 800) // Check if player has enough gold
        {
            PlayerController.goldCount -= 800;
            PlayerController.ExplosiveBought = true;
            OnBuyExplosiveWand?.Invoke(800); // Triggers to buy Weapon
            Debug.Log("Explosive Wand bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

}

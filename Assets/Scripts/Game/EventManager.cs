using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

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

    public void BuyCannonTower()
    {
        if (PlayerController.goldCount >= 50) // Check if player has enough gold
        {
            PlayerController.goldCount -= 50; 
            OnBuyCannonTower?.Invoke(50); // Triggers to buy tower
            Debug.Log("Cannon Tower bought! Remaining gold: " + PlayerController.goldCount);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}

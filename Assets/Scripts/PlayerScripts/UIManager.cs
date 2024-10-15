using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar; 
    public Image manaBar;
    private int health;
    private int maxHealth;

    private WandShooting wandShooting; // Reference to Wand script

    public GameObject PlayerUI;
    public GameObject WinScreen;
    public GameObject LoseScreen;
    

    void Start()
    {
        health = PlayerController.Instance.currHealth;
        maxHealth = PlayerController.Instance.maxHealth;

        Debug.Log(health);
        Debug.Log(maxHealth);
        Debug.Log(health/maxHealth);
    }
    void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    void UpdateHealthBar()
    {
        
        Debug.Log(healthBar.fillAmount);
        healthBar.fillAmount = (float)health / maxHealth;
        
        if (health <= 0)
        {
            LoseScreen.SetActive(true);
            PlayerUI.SetActive(false);
        }
        
    }

    void UpdateManaBar()
    {
        if (wandShooting != null)
        {
           
        }
    }



}
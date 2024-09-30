using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar; // Drag your HealthBar image here in the inspector
    public Image manaBar; // Drag your ManaBar image here in the inspector
    public TMP_Text manaAmount;
    public TMP_Text healthAmount;
    public PlayerController playerController; // Reference to playerController
    public WandShooting wandShooting; // Reference to Wand script

    void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    void UpdateHealthBar()
    {
        if (playerController != null)
        {
            healthAmount.text = ("Health: " + playerController.currHealth.ToString() + "/" + playerController.maxHealth.ToString());
            if (playerController.currHealth <= 0)
            {
                healthAmount.text = ("You Died!");
            }
        }
    }

    void UpdateManaBar()
    {
        if (wandShooting != null)
        {
            manaAmount.text = ("Mana: " + wandShooting.currMana.ToString() + "/" + wandShooting.maxMana.ToString());
            if (wandShooting.currMana == 0)
            {
                manaAmount.text = ("Press R to Reload!");
            }
        }
    }

   
}
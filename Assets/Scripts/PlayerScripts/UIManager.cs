using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar; // Drag your HealthBar image here in the inspector
    public Image manaBar; // Drag your ManaBar image here in the inspector

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
            float healthPercentage = playerController.currHealth / playerController.maxHealth;
            healthBar.fillAmount = Mathf.Clamp01(healthPercentage);
        }
    }

    void UpdateManaBar()
    {
        if (wandShooting != null)
        {
            float manaPercentage = wandShooting.currMana / wandShooting.maxMana;
            manaBar.fillAmount = Mathf.Clamp01(manaPercentage);
        }
    }
}
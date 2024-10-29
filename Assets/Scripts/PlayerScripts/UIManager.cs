using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    public Image manaBar;
    public Image kingdomHealthBar;
    public GameObject wand;

    private WandShooting wandShooting; // Reference to Wand script

    public GameObject PlayerUI;
    public GameObject WinScreen;
    public GameObject LoseScreen;


    private void Start()
    {
        wandShooting = wand.GetComponent<WandShooting>();
    }
    void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
        UpdateKingdomHealthBar();
    }
    

    public void UpdateHealthBar()
    {

        healthBar.fillAmount = (float)PlayerController.Instance.currHealth / PlayerController.Instance.maxHealth;

        if (PlayerController.Instance.currHealth <= 0)
        {
            LoseScreen.SetActive(true);
            PlayerController.Instance.EndGame();
        }

    }

    public void UpdateManaBar()
    {
        if (wandShooting != null)
        {
            manaBar.fillAmount = (float)wandShooting.currMana / wandShooting.maxMana;
        }
    }

    public void UpdateKingdomHealthBar()
    {
        kingdomHealthBar.fillAmount = (float)PlayerController.Instance.KingdomHealth / PlayerController.Instance.kingdomMaxHealth;

        if (PlayerController.Instance.KingdomHealth <= 0)
        {
            LoseScreen.SetActive(true);
            PlayerController.Instance.EndGame();
        }
    }


}
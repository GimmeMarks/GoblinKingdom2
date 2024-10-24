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
    }

    public void UpdateHealthBar()
    {

        healthBar.fillAmount = (float)PlayerController.Instance.currHealth / PlayerController.Instance.maxHealth;

        if (PlayerController.Instance.currHealth <= 0)
        {
            LoseScreen.SetActive(true);
            PlayerUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
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

    }


}
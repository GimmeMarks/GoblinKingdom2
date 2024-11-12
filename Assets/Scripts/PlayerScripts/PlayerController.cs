using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class PlayerController : MonoBehaviour
{
    [Tab("Movement")]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float gravity;
    public float speed = 5.0f;
    public float jumpForce = 3f;
    public static int goldCount = 1000; //Got to start at ground zero
    [Tab("Serializables")]

    public TMP_Text waveNumberUI;
    public TMP_Text goldTextUI;
    public Slider SensSlider;
    
    [Tab("PlayerStats")]
    //Stats that can be changed buy NPCs
    public int maxHealth = 100;
    public int currHealth = 100;
    public int KingdomHealth = 100;
    public int kingdomMaxHealth = 100;
    [EndTab]

    [Tab("Sounds")]
    public AudioClip jumpSound;
    public AudioClip footstepSound;
    public AudioSource audioSource;
    [EndTab]

    public bool inMenu = false;
    public GameObject PlayerUI;
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    
    //NPC Tower Buys (Allows to check if a tower is bought or not)
    public bool inConversation = false;
    public static bool Bought1 = false;
    public static bool Bought2 = false;
    public static bool Bought3 = false;
    public static bool Bought4 = false;
    public static bool Bought5 = false;
    public static bool Bought6 = false;
    public static bool Bought7 = false;
    public static bool Bought8 = false;
    public static bool LaserBought = false;
    public static bool IceBought = false;
    public static bool ExplosiveBought = false;


    // Singleton instance
    public static PlayerController Instance { get; private set; }

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



        //---------------------------------------------------------------------
        //Event NPC stuuf:
        //---------------------------------------------------------------------

        //Tower Buys
        EventManager.Instance.OnBuyCannonTower += UpdateGoldUI; // Subscribe to the cannon event
        EventManager.Instance.OnBuyMageTower += UpdateGoldUI; // Subscribe to the mage event
        EventManager.Instance.OnBuyTrapTower += UpdateGoldUI; // Subscribe to the trap event

        //Upgrade Buys
        EventManager.Instance.OnBuyHealthUp += UpdateGoldUI; // Subscribe to the health upgrade event
        EventManager.Instance.OnBuyManaUp += UpdateGoldUI; // Subscribe to the mana upgrade event
        EventManager.Instance.OnBuyDamageUp += UpdateGoldUI; // Subscribe to the damage upgrade event
        EventManager.Instance.OnBuyRegenUp += UpdateGoldUI; // Subscribe to the regen event
        EventManager.Instance.OnBuySpeedUp += UpdateGoldUI; // Subscribe to the speed upgrade event

        //Heath Buys
        EventManager.Instance.OnBuyHealthS += UpdateGoldUI; // Subscribe to the Health small event
        EventManager.Instance.OnBuyHealthL += UpdateGoldUI; // Subscribe to the Health large event
        EventManager.Instance.OnBuyHealthM += UpdateGoldUI; // Subscribe to the Health max event

        //Weapon Buys
        EventManager.Instance.OnBuyLaserWand += UpdateGoldUI; // Subscribe to the Laser wand event
        EventManager.Instance.OnBuyIceWand += UpdateGoldUI; // Subscribe to the Ice wand event
        EventManager.Instance.OnBuyExplosiveWand += UpdateGoldUI; // Subscribe to the Explosive wand event


    }

    public void SetSensitivity()
    {
        lookSpeed = SensSlider.value;
    }

    private bool isMoving = false;  // To track if the player is moving
    void Update()
    {
        // Movement
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.y = moveDirectionY;
        goldTextUI.text = goldCount.ToString();

        bool moving = moveDirection.x != 0 || moveDirection.z != 0;  // Check if there's horizontal or vertical movement


        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce; // This can be adjusted for jump force
                PlaySound(jumpSound);
            }
            else
            {
                moveDirection.y = 0;
                
            }
        }

            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * speed * Time.deltaTime);
            
        if(moving && !isMoving)
        {
            Debug.Log("FOOTSTEPS");
            isMoving = true;
            PlaySound(footstepSound);
        }


        // Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Horizontal rotation (Y-axis)
        float rotationY = Input.GetAxis("Mouse X") * lookSpeed;
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);

    }

    public void TakeDamage(int enemyDamage)
    {
        currHealth -= enemyDamage;
        Debug.Log("Health = " + currHealth);

    }
    private void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Show the cursor
    }
    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
    }


    public void EndGame()
    {
        UnlockMouse();
        PlayerUI.SetActive(false);
        inMenu = true;
        this.enabled = false;
    }
    public void ResumeGame()
    {
        LockMouse();
        PlayerUI.SetActive(true);
        inMenu = false;
        this.enabled = true;
    }
    
    //Unfinished
    /*
    IEnumerator PlayerHealthRegen()
    {
        Mathf.Clamp(currHealth, 0, maxHealth);
        
        while (true){
            currHealth += Math.Round(rou)
        }


        yield return new WaitForSeconds(5);
    }
    */

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gold"))
        {
            goldScript goldCode = other.GetComponent<goldScript>();
            goldCount += goldCode.goldAmount;
            goldCode.DestroyMe();

        }

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            inConversation = true;

        }

    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            inConversation = false;

        }

    }


    



    //-----------------------------------------------------------
    //NPC Buys
    //-----------------------------------------------------------

    //Tower Buys
    public void OnBuyCannonTowerLTButtonPressed()
    {
        EventManager.Instance.BuyCannonTowerLT();
    }
    public void OnBuyCannonTowerRTButtonPressed()
    {
        EventManager.Instance.BuyCannonTowerRT();
    }
    public void OnBuyCannonTowerLBButtonPressed()
    {
        EventManager.Instance.BuyCannonTowerLB();
    }
    public void OnBuyCannonTowerRBButtonPressed()
    {
        EventManager.Instance.BuyCannonTowerRB();
    }

    public void OnBuyMageTowerTButtonPressed()
    {
        EventManager.Instance.BuyMageTowerT();
    }
    public void OnBuyMageTowerBButtonPressed()
    {
        EventManager.Instance.BuyMageTowerB();
    }

    public void OnBuyTrapTowerLButtonPressed()
    {
        EventManager.Instance.BuyTrapTowerL();
    }

    public void OnBuyTrapTowerRButtonPressed()
    {
        EventManager.Instance.BuyTrapTowerR();
    }

    //Upgrade Buys
    public void OnBuyHealthUpButtonPressed()
    {
        EventManager.Instance.BuyHealthUp();
    }
    public void OnBuyManaUpButtonPressed()
    {
        EventManager.Instance.BuyManaUp();
    }
    public void OnBuyDamageUpButtonPressed()
    {
        EventManager.Instance.BuyDamageUp();
    }
    public void OnBuyRegenUpButtonPressed()
    {
        EventManager.Instance.BuyRegenUp();
    }
    public void OnBuySpeedUpButtonPressed()
    {
        EventManager.Instance.BuySpeedUp();
    }

    //Health Buys
    public void OnBuyHealthSButtonPressed()
    {
        EventManager.Instance.BuyHealthS();
    }
    public void OnBuyHealthLButtonPressed()
    {
        EventManager.Instance.BuyHealthL();
    }
    public void OnBuyHealthMButtonPressed()
    {
        EventManager.Instance.BuyHealthM();
    }

    //Weapon Buys
    public void OnBuyLaserWandButtonPressed()
    {
        EventManager.Instance.BuyLaserWand();
    }
    public void OnBuyIceWandButtonPressed()
    {
        EventManager.Instance.BuyIceWand();
    }
    public void OnBuyExplosiveWandButtonPressed()
    {
        EventManager.Instance.BuyExplosiveWand();
    }

    private void UpdateGoldUI(int amount)
    {

    }

}


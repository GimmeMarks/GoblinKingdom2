using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float speed = 5.0f;
    private float lookSpeed = 2.0f;
    private float lookXLimit = 45.0f;
    private float gravity = 9.8f;
    public TMP_Text waveNumberUI;
    public TMP_Text goldTextUI;
    public int roundNumGlobal;
    private UIManager UIMANAGER;
    

    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public int maxHealth = 100;
    public int currHealth = 100;
    public int currKingHealth = 500;
    public int maxKingHealth = 500;

    public static int goldCount = 50;


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

        UIMANAGER = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Event stuuf
        EventManager.Instance.OnBuyCannonTower += UpdateGoldUI; // Subscribe to the event
        EventManager.Instance.OnBuyMageTower += UpdateGoldUI; // Subscribe to the event

    }

    void Update()
    {
        // Movement
        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.y = moveDirectionY;
        goldTextUI.text = goldCount.ToString();

        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speed; // This can be adjusted for jump force
            }
            else
            {
                moveDirection.y = 0;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * speed * Time.deltaTime);

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
        if (UIMANAGER != null)
        {
            UIMANAGER.UpdateHealthBar();
        }
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


    public void roundManager(int roundNum)
    {
        roundNumGlobal = roundNum;
        waveNumberUI.text = ("Round: " + (roundNum + 1).ToString());
        
    }

    public void OnBuyCannonTowerButtonPressed()
    {
        EventManager.Instance.BuyCannonTower();
    }
    public void OnBuyMageTowerButtonPressed()
    {
        EventManager.Instance.BuyMageTower();
    }

    private void UpdateGoldUI(int amount)
    {
        // This can update your UI
        Debug.Log("Bought Cannon Tower, gold deducted: " + amount);

    }

}
    

using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public float gravity = 9.8f;

    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public TextMeshProUGUI deathMessage; // Assign in the Inspector
    private bool isDead = false;

    public static PlayerController Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("PlayerController").AddComponent<PlayerController>();
            return instance;
        }
    }
    private static PlayerController instance = null;

    void Awake()
    {
        if ((instance) && (instance.GetInstanceID() != GetInstanceID()))
            DestroyImmediate(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        deathMessage.gameObject.SetActive(false); // Hide the death message at start
    }

    void Update()
    {
        if (isDead) return; // Stop any movement if dead

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection.y = moveDirectionY;

        if (characterController.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speed; // Adjust for jump force
            }
            else
            {
                moveDirection.y = 0;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * speed * Time.deltaTime);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float rotationY = Input.GetAxis("Mouse X") * lookSpeed;
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);
    }

    public void TakeDamage(int enemyDamage)
    {
        HealthSystem.Instance.TakeDamage(enemyDamage); // Call the HealthSystem's TakeDamage
        Debug.Log("Health = " + HealthSystem.Instance.hitPoint);

        if (HealthSystem.Instance.hitPoint <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Set dead flag
        deathMessage.gameObject.SetActive(true); // Show death message
        deathMessage.text = "You Died!";

        // Stop player movement and make them fall
        characterController.enabled = false; // Disable movement
        transform.rotation = Quaternion.Euler(-90, 0, 0); // Rotate player to fall over

        GameManager.Instance.RestartGame(); // Call the GameManager to restart the game
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("You Hit..." + other.gameObject);
        // Check if the collider belongs to an enemy
        if (other.CompareTag("enemy"))
        {
            EnemyBaseClass enemy = other.GetComponent<EnemyBaseClass>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Call the TakeDamage method on the enemy
            }
            Destroy(gameObject); // Destroy the bullet after it hits
        }
        else if (other.CompareTag("Player")) // Optional: If you want the bullet to be destroyed on hitting the player
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("OBJ")) // Destroy the bullet on any other collision
        {
            Destroy(gameObject);
        }
    }
}
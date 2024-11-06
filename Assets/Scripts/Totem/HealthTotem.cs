using UnityEngine;

public class HealthTotem : MonoBehaviour
{
    public int healthIncrease = 20;                // Amount to increase health
    public GameObject particleEffectPrefab;        // Particle effect to show when collected
    private PlayerController PlayerController;

    // Called when the player collects the totem
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            // Apply effect (increase health)
            PlayerController.maxHealth += healthIncrease;
            Debug.Log("Health increased by " + healthIncrease);

            // Spawn the particle effect and destroy the totem
            SpawnParticleEffect();
        }
    }

    // Method to spawn the particle effect and destroy the totem
    private void SpawnParticleEffect()
    {
        if (particleEffectPrefab != null)
        {
            // Instantiate particle effect at the totem's position
            GameObject particleEffect = Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);
            Destroy(particleEffect, 1f); // Destroy particle effect after 1 second
        }

        // Destroy the totem object
        Destroy(gameObject);
    }
}
using UnityEngine;

public class DamageTotem : MonoBehaviour
{
    public int damageIncrease = 5;                 // Amount to increase damage
    public GameObject particleEffectPrefab;        // Particle effect to show when collected
    private EventManager EventManager;

    // Called when the player collects the totem
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            // Apply effect (increase damage)
            EventManager.damageBuff += 1;
            Debug.Log("Damage increased by " + damageIncrease);

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
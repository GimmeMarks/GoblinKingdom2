using UnityEngine;

public class GoldTotem : MonoBehaviour
{
    public int goldIncrease = 20;                  // Amount to increase gold
    public GameObject particleEffectPrefab;        // Particle effect to show when collected
    private PlayerController PlayerController;

    // Called when the player collects the totem
    private void OnTriggerEnter(Collider other)
    {
        PlayerController = other.GetComponent<PlayerController>();

        if (other.CompareTag("Player"))
        {
            // Apply effect (increase gold)
            PlayerController.goldCount += goldIncrease;
            Debug.Log("Gold increased by " + goldIncrease);

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
            Destroy(particleEffect, 3f); // Destroy particle effect after 1 second
        }

        // Destroy the totem object
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.XR;

public class ManaTotem : MonoBehaviour
{
    public int manaIncrease = 20;                  // Amount to increase mana
    public GameObject particleEffectPrefab;        // Particle effect to show when collected
    private WandShooting WandShooting;

        // Called when the player collects the totem
        private void OnTriggerEnter(Collider other)
    {

        WandShooting = other.GetComponent<WandShooting>();

        if (other.CompareTag("Player"))
        {
            // Apply effect (increase mana)
            WandShooting.maxMana += manaIncrease;
            Debug.Log("Mana increased by " + manaIncrease);

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
using UnityEngine;
using System.Collections;

public class TotemSpawner : MonoBehaviour
{
    public GameObject[] totemPrefabs;       // Array to hold different totem prefabs
    public Transform[] spawnLocations;      // Array of spawn locations
    public float minSpawnTime = 30f;         // Minimum spawn time
    public float maxSpawnTime = 120f;        // Maximum spawn time
    public float totemLifetime = 30f;        // Lifetime of the totem before it despawns (in seconds)

    private GameObject currentTotem;        // Reference to the currently spawned totem
    private bool totemActive = false;       // Flag to track if a totem is currently active

    void Start()
    {
        // Start the first spawn cycle
        StartCoroutine(SpawnTotemCoroutine());
    }

    // Coroutine to spawn totems after a random time interval
    private IEnumerator SpawnTotemCoroutine()
    {
        while (true)
        {
            if (!totemActive)
            {
                // Wait for a random time between minSpawnTime and maxSpawnTime
                float spawnDelay = Random.Range(minSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnDelay);

                // Spawn the totem at a random spawn location
                SpawnTotem();
            }
            else
            {
                // If the totem is active, wait until it's collected or the lifetime expires
                yield return null;
            }
        }
    }

    // Method to spawn the totem
    private void SpawnTotem()
    {
        // Pick a random spawn location
        int randomSpawnIndex = Random.Range(0, spawnLocations.Length);
        Transform spawnPoint = spawnLocations[randomSpawnIndex];

        // Pick a random totem prefab from the array
        int randomTotemIndex = Random.Range(0, totemPrefabs.Length);
        GameObject randomTotemPrefab = totemPrefabs[randomTotemIndex];

        // Instantiate the chosen totem at the chosen spawn point
        currentTotem = Instantiate(randomTotemPrefab, spawnPoint.position, Quaternion.identity);
        totemActive = true;

        // Start a coroutine to destroy the totem after a set amount of time (totemLifetime)
        StartCoroutine(DestroyTotemAfterLifetime());
    }

    // Coroutine to destroy the totem after a specific time
    private IEnumerator DestroyTotemAfterLifetime()
    {
        // Wait for the duration of the totem's lifetime
        yield return new WaitForSeconds(totemLifetime);

        // Destroy the totem if it still exists (in case the player hasn't collected it)
        if (currentTotem != null)
        {
            Destroy(currentTotem);
            totemActive = false;
        }
    }

    // Call this method when the player collects the totem
    public void CollectTotem()
    {
        if (currentTotem != null)
        {
            Destroy(currentTotem);  // Destroy the totem when collected
            totemActive = false;    // Reset the flag to allow the next spawn
        }
    }
}

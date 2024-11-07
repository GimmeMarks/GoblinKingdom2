using UnityEngine;

public class RandomSeedGen : MonoBehaviour
{
    void Start()
    {
        // Seed the random number generator with the current time in milliseconds
        int seed = System.DateTime.Now.Millisecond;
        Random.InitState(seed);

        // Now you can use Random for any random operations
        float randomValue = Random.Range(0f, 1f);
        Debug.Log("Random value: " + randomValue);
    }
}
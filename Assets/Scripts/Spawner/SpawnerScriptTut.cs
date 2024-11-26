using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerScriptTut : MonoBehaviour
{
    //Initalizing Variables
    int waveNumber;

    // ----------------------------------------
    // Win Game Amount
    // ----------------------------------------

    // ----------------------------------------

    [SerializeField] Transform[] redSpawnLocations;
    [SerializeField] Transform[] blueSpawnLocations;
    [SerializeField] GameObject smallEnemy;
    [SerializeField] GameObject mediumEnemy;
    [SerializeField] GameObject largeEnemy;
    [SerializeField] GameObject riderEnemy;
    [SerializeField] GameObject flyerEnemy;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] TMP_Text roundText;
    [SerializeField] GameObject countdownTextObject;
    [SerializeField] GameObject winScreen;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject bridgeBarrier;
    //Base Locations
    public Transform baseLocation1;
    public Transform baseLocation2;
    public Transform playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        //Set Wave number
        waveNumber = 1;
        StartCoroutine("WaveManager");

    }
    IEnumerator WaveManager()
    {
        int nextWaveStartTime = 3; // WaveDelay

        while (true)
        {
            // Countdown before starting the next wave
            while (nextWaveStartTime >= 0)
            {
                countdownTextObject.SetActive(true);
                roundText.text = ("Defend the Kingdom").ToString();
                countdownText.text = "Round Starts in " + nextWaveStartTime.ToString();
                nextWaveStartTime--;

                // Wait for 1 second before continuing
                yield return new WaitForSeconds(1f);

            }

            countdownTextObject.SetActive(false);

            //Smallest = Wavenumber * 3, Largest = Wavenumber * 5
            int enemyBudget = Random.Range(1 * 3 + 5, 1 * 5 + 10);
            //Start the coroutine to spawn enemies by feeding it the value generated
            yield return StartCoroutine(SpawnEnemies(enemyBudget));
            //Wait until the game can't find any enemies left

            yield return new WaitUntil(() => AllEnemiesDefeated());
            roundText.text = ("Nice Job! See the Goblin at the Kingdom.");
            break;
        }
    }

    //Spawns enemies based on the current budget
    IEnumerator SpawnEnemies(int enemyBudget)
    {
        bool spawnPoint = SpawnPointGenerator();

        Transform spawnLocation = blueSpawnLocations[Random.Range(0, blueSpawnLocations.Length)];
        while (enemyBudget > 0)
        {
            //Pick a random enemy type based on the budget (different enemies cost different amounts)
            GameObject enemyToSpawn = GetEnemyBasedOnBudget(ref enemyBudget);
            //if there are more enemies to spawn, pick a random location and spawn em!
            if (enemyToSpawn != null)
            {
                spawnLocation = blueSpawnLocations[Random.Range(0, blueSpawnLocations.Length)];

                //spawn
                GameObject enemyInstance = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.identity);
                //Get the instantiated enemies baseClass script
                EnemyBaseClass enemyScript = enemyInstance.GetComponent<EnemyBaseClass>();
                //assign them the location of the base points
                enemyScript.baseTransform1 = baseLocation1;
                enemyScript.baseTransform2 = baseLocation2;
                enemyScript.playerTransform = playerLocation;

            }

            //Delays enemy spawns
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    //Checks if all enemies are dead
    bool AllEnemiesDefeated()
    {
        bridgeBarrier.SetActive(false);
        //Do any game objects of type enemy exist?
        return FindObjectsOfType<EnemyBaseClass>().Length == 0;
    }
    bool SpawnPointGenerator()
    {
        roundText.text = ("Defend the Kingdom");
        return true;
    }

    //Select an enemy type based on the available budget
    GameObject GetEnemyBasedOnBudget(ref int enemyBudget)
    {
        //Defining enemy types and their costs
        GameObject[] enemyTypes = { smallEnemy, mediumEnemy, largeEnemy, riderEnemy, flyerEnemy };
        int[] enemyCosts = { 1, 3, 10, 5, 1 };

        //Select a random enemy type that fits within the current budget
        int randomIndex = Random.Range(0, enemyTypes.Length);

        // if the enemy we select fits the budget
        if (enemyBudget >= enemyCosts[randomIndex])
        {
            //subtract the enemies cost from our budget
            enemyBudget -= enemyCosts[randomIndex];
            //return the enemy type selected
            return enemyTypes[randomIndex];
        }
        //if u cant spawn the enemy, bye
        return null;
    }



}
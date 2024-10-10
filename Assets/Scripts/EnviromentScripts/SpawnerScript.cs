using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    //Initalizing Variables
    int waveNumber;
    [SerializeField] Transform[] spawnLocations;
    [SerializeField] GameObject smallEnemy;
    [SerializeField] GameObject mediumEnemy;
    [SerializeField] GameObject largeEnemy;
    [SerializeField] GameObject riderEnemy;
    [SerializeField] GameObject flyerEnemy;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] GameObject countdownTimerUIObject;
    //Base Locations
    public Transform baseLocation1; 
    public Transform baseLocation2;
    public Transform playerLocation;

    // Start is called before the first frame update
    void Start()
    {
        //Set Wave number
        waveNumber = 0;
        //call this to update the wave number UI
        PlayerController.Instance.roundManager(waveNumber);
        StartCoroutine("WaveManager");

    }
    IEnumerator WaveManager()
    {
        int nextWaveStartTime = 30; // WaveDelay

        while (true)
        {
            // Countdown before starting the next wave
            while (nextWaveStartTime >= 0)
            {
                countdownTimerUIObject.SetActive(true);
                //Tick down the time
                countdownText.text = ("Round Starts in " + nextWaveStartTime.ToString());
                yield return new WaitForSeconds(1);
                nextWaveStartTime--;
            }
            countdownTimerUIObject.SetActive(false);
            //Smallest = Wavenumber * 3, Largest = Wavenumber * 5
            int enemyBudget = Random.Range(waveNumber * 3 + 5, waveNumber * 5 + 10);
            //Start the coroutine to spawn enemies by feeding it the value generated
            StartCoroutine(SpawnEnemies(enemyBudget));
            //Wait until the game can't find any enemies left
            yield return new WaitUntil(() => AllEnemiesDefeated());
            //Once all enemies are dead, increase wave counter and push to the UI element
            waveNumber++;
            PlayerController.Instance.roundManager(waveNumber);
            nextWaveStartTime = 30;
        }
    }

    //Spawns enemies based on the current budget
    IEnumerator SpawnEnemies(int enemyBudget)
    {
        while (enemyBudget > 0)
        {
            //Pick a random enemy type based on the budget (different enemies cost different amounts)
            GameObject enemyToSpawn = GetEnemyBasedOnBudget(ref enemyBudget);
            //if there are more enemies to spawn, pick a random location and spawn em!
            if (enemyToSpawn != null)
            {
                //Picking random spawn location
                Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Length)];
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
        //Do any game objects of type enemy exist?
        return FindObjectsOfType<EnemyBaseClass>().Length == 0;
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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Enemy prefabs
    public GameObject regularEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    // Wave settings
    public float timeBeforeStart ;
    public float timeBetweenWaves = 10f;
    public int totalNumberOfWaves = 10;
    public int baseEnemyCount = 5;
    public int enemyCountIncrement = 2;


    // Other variables
    private int currentWaveNumber = 1;
    private bool isSpawning = false;
    public Castle castle;

    // Reference to coroutine
    private Coroutine spawnCoroutine;
    private Coroutine startSpawningCoroutine;
    // List of enemies to spawn in the current wave
    private List<EnemyType> enemiesToSpawn;

    void Start()
    {
        startSpawningCoroutine = StartCoroutine(StartSpawning());
    }

    private float GetPlayerPerformance()
    {
        if (castle != null)
        {
            return castle.GetHealthPercentage(); // Value between 0 and 1
        }
        else
        {
            UnityEngine.Debug.LogWarning("Player reference is not set in EnemySpawn.");
            return 1f; // Assume full health if player reference is missing
        }
    }

        public IEnumerator StartSpawning()
    {
        enemiesToSpawn = GenerateEnemiesForWave(currentWaveNumber);
        // Wait before starting the first wave
        yield return new WaitForSeconds(timeBeforeStart);

        while (currentWaveNumber <= totalNumberOfWaves)
        {
            UnityEngine.Debug.Log("Starting Wave " + currentWaveNumber);
            isSpawning = true;

            // Generate enemies for the current wave
            enemiesToSpawn = GenerateEnemiesForWave(currentWaveNumber);

            // Start spawning enemies
            spawnCoroutine = StartCoroutine(SpawnEnemies());

            // Wait until all enemies are spawned
            yield return new WaitUntil(() => enemiesToSpawn.Count == 0);

            // Wait until all enemies are destroyed before starting the next wave
            yield return new WaitUntil(() => Enemies.enemies.Count == 0);

            isSpawning = false;
            currentWaveNumber++;

            if (currentWaveNumber <= totalNumberOfWaves)
            {
                UnityEngine.Debug.Log("Wave completed. Next wave in " + timeBetweenWaves + " seconds.");
                yield return new WaitForSeconds(timeBetweenWaves);
            }
            else
            {
                UnityEngine.Debug.Log("All waves completed!");
            }
        }
    }

    List<EnemyType> GenerateEnemiesForWave(int waveNumber)
    {
        List<EnemyType> enemyList = new List<EnemyType>();
        int enemyCount = baseEnemyCount + (waveNumber - 1) * enemyCountIncrement;

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyType enemyType = DecideEnemyType(waveNumber);
            enemyList.Add(enemyType);
        }

        return enemyList;
    }

    EnemyType DecideEnemyType(int waveNumber)
    {
        float playerPerformance = GetPlayerPerformance();
        float rand = UnityEngine.Random.value;

        // Wave 1
        if (waveNumber < 2)
        {
            return EnemyType.Regular;
        }
        // Waves 2-3
        else if (waveNumber < 4)
        {
            if (playerPerformance >= 0.8f)
            {
                // Player is doing well; introduce challenging enemies
                if (rand < 0.7f)
                    return EnemyType.Fast;
                else
                    return EnemyType.Regular;
            }
            else if (playerPerformance >= 0.3f)
            {
                // Player is average
                if (rand < 0.7f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
            else
            {
                if (rand < 0.85f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
        }
        // Waves 4 and above
        else
        {
            if (playerPerformance >= 0.8f)
            {
                // Player is doing well; higher chance of tougher enemies
                if (rand < 0.5f)
                    return EnemyType.Fast;
                else if (rand < 0.8f)
                    return EnemyType.Tank;
                else
                    return EnemyType.Regular;
            }
            else if (playerPerformance >= 0.3f)
            {
                // Player is average; balanced enemy types
                if (rand < 0.4f)
                    return EnemyType.Fast;
                else if (rand < 0.6f)
                    return EnemyType.Tank;
                else
                    return EnemyType.Regular;
            }
            else
            {
                // Player is struggling; spawn easier enemies
                if (rand < 0.80f)
                    return EnemyType.Regular;
                else
                    return EnemyType.Fast;
            }
        }
    }


    public IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn.Count > 0)
        {

            if (enemiesToSpawn == null)
            {
                UnityEngine.Debug.LogError("enemiesToSpawn is null in SpawnEnemies!");
                yield break;
            }
            EnemyType enemyType = enemiesToSpawn[0];
            enemiesToSpawn.RemoveAt(0);

            SpawnEnemy(enemyType);

            // Adjust the spawn interval as needed
            yield return new WaitForSeconds(1f);
        }
    }

    void SpawnEnemy(EnemyType enemyType)
    {
        GameObject enemyPrefab = GetEnemyPrefab(enemyType);

        int randomIndex = UnityEngine.Random.Range(0, MapGenerator.startTile.Length);
        GameObject spawnTile = MapGenerator.startTile[randomIndex];

        if (spawnTile != null)
        {
            Instantiate(enemyPrefab, spawnTile.transform.position, Quaternion.identity);
        }
    }

    GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Regular:
                return regularEnemyPrefab;
            case EnemyType.Fast:
                return fastEnemyPrefab;
            case EnemyType.Tank:
                return tankEnemyPrefab;
            default:
                return regularEnemyPrefab;
        }
    }
    public void StopSpawning()
    {
        // Stop the main spawning coroutine
        if (startSpawningCoroutine != null)
        {
            StopCoroutine(startSpawningCoroutine);
            startSpawningCoroutine = null;
        }

        // Stop the enemy spawning coroutine if it's running
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        isSpawning = false;
    }
    void Update()
    {
        Enemy.CleanUpDestroyedEnemies();
    }
}
// EnemyType enum should be in a separate script file EnemyType.cs
public enum EnemyType
{
    Regular,
    Fast,
    Tank
}
